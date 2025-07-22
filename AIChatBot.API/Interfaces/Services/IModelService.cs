
using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.Services
{

    public interface IModelService
    {
        List<AIModel> GetAllModelsAsync();
        AIModel GetModelById(int modelId);
    }
}
