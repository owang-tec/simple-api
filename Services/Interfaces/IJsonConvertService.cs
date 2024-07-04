using op.Models;

namespace op.Services.Interfaces
{
    public interface IJsonConvertService
    {
        string ConvertXmlToJson(EvaluationModel evaluation);
    }
}
