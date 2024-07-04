using Microsoft.Extensions.Options;
using op.Models;
using op.Services.Interfaces;
using System.Xml.Schema;

namespace op.Services
{
    public class FetchDataService : IFetchDataService
    {
        private readonly ILogger<FetchDataService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJsonConvertService _jsonConvertService;
        private readonly IXmlService _xmlService;
        private readonly IOptionsMonitor<ApiSettings> _apiSettings;

        public FetchDataService(
            IOptionsMonitor<ApiSettings> apiSettings, 
            IJsonConvertService jsonConvertService, 
            IXmlService xmlService,
            IHttpClientFactory httpClientFactory, 
            ILogger<FetchDataService> logger)
        {
            _apiSettings = apiSettings;
            _jsonConvertService = jsonConvertService;
            _xmlService = xmlService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<EvaluationModel> FetchDataAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var baseUrl = _apiSettings.CurrentValue.BaseUrl;
            var url = $"{baseUrl}{id}.xml";

            int retryAttemps = 3;
            int delayMilliseconds = 1000;

            for (int attempt = 0; attempt < retryAttemps; attempt++)
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var xmlContent = await response.Content.ReadAsStringAsync();

                    _xmlService.ValidateXml(xmlContent);

                    EvaluationModel parsedContent = _xmlService.ParseXml(xmlContent);

                    return parsedContent;
                }
                catch (HttpRequestException ex) when (IsTransientHttpError(ex))
                {
                    _logger.LogError(ex, "Transient error occurred while fetching data from API");

                    await Task.Delay(delayMilliseconds);
                    throw;
                }
                catch (HttpRequestException httpRequestException)
                {
                    _logger.LogError(httpRequestException, "Error getting data from API");
                    throw;
                }
                catch (XmlSchemaValidationException xmlValidationException)
                {
                    _logger.LogError(xmlValidationException, "XML validation error");
                    throw;
                }
            }

            throw new HttpRequestException($"Failed to fetch data from API after {retryAttemps} attempts");   
        }

        private bool IsTransientHttpError(HttpRequestException ex)
        {
            return ex.InnerException is System.Net.Sockets.SocketException;
        }
    }
}
