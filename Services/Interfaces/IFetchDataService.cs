using Microsoft.AspNetCore.Mvc;
using op.Models;

namespace op.Services.Interfaces
{
    public interface IFetchDataService
    {
        Task<EvaluationModel> FetchDataAsync(int id);
    }
}
