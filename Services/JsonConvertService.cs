using Newtonsoft.Json;
using op.Models;
using op.Services.Interfaces;

namespace op.Services
{
    public class JsonConvertService : IJsonConvertService
    {
        public string ConvertXmlToJson(EvaluationModel evaluation)
        {
            var jsonObject = new
            {
                evaluation.Id,
                evaluation.Name,
                evaluation.Description
            };

            var jsonContent = JsonConvert.SerializeObject(jsonObject);

            return jsonContent;
        }
    }
}
