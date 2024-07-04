using Microsoft.AspNetCore.Mvc;
using op.Models;

namespace op.Services.Interfaces
{
    public interface IXmlService
    {
        void ValidateXml(string xmlContent);

        EvaluationModel ParseXml(string xmlContent);
    }
}
