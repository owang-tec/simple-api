using Microsoft.AspNetCore.Mvc;
using op.Services.Interfaces;

namespace op.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DataTransformController : ControllerBase
    {
        private readonly IJsonConvertService _jsonConvertService;
        private readonly IFetchDataService _fetchDataService;

        public DataTransformController(IJsonConvertService jsonConvertService, IFetchDataService fetchDataService)
        {
            _jsonConvertService = jsonConvertService;
            _fetchDataService = fetchDataService;
        }

        /// <summary>
        /// Fetch the data from the API and convert it to JSON.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches the data from the API, evalutes it against the xml schema schema.xsd and converts it to JSON.
        /// </remarks>
        /// <param name="id">The unique identifier used to fetch data from the API.</param>
        /// <returns>A JSON representation of the fetched data.</returns>
        [HttpGet(Name = "id")]
        public async Task<IActionResult> FetchAndConvertXmlToJson(int id)
        {
            var xmlContent = await _fetchDataService.FetchDataAsync(id);
            var jsonContent = _jsonConvertService.ConvertXmlToJson(xmlContent);

            return Ok(jsonContent);
        }
    }
}
