using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AIChatBot.API.Services
{
    public class ModelService : IModelService
    {
        private readonly ChatBotDbContext _dbContext;
        private string cacheKey = "ModelsSession";
        private readonly IMemoryCache _cache;

        public ModelService(ChatBotDbContext chatBotDbContext, IMemoryCache cache)
        {
            _dbContext = chatBotDbContext;
            _cache = cache;
        }
        public List<AIModel> GetAllModelsAsync()
        {
            if (_cache.TryGetValue(cacheKey, out List<AIModel> models))
            {
                return models;
            }

            models = _dbContext.AIModels
                .Include(m => m.ChatModes)
                .ThenInclude(cm => cm.ChatMode)
                .ToList();

            _cache.Set(cacheKey, models);
            return models;
        }

        public AIModel GetModelById(int modelId)
        {
            if (_cache.TryGetValue(cacheKey, out List<AIModel> models))
            {
                return models.FirstOrDefault(m => m.Id == modelId);
            }

            var model = _dbContext.AIModels
                .Include(m => m.ChatModes)
                .ThenInclude(cm => cm.ChatMode)
                .FirstOrDefault(m => m.Id == modelId);

            return model;
        }
    }
}
