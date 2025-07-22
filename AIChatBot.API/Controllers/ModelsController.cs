using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModelsController : ControllerBase
    {
        
        private readonly IModelService _modelService;
        public ModelsController(ChatBotDbContext dbContext, IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public IActionResult GetModels()
        {
            var models = _modelService.GetAllModelsAsync();

            if (models == null || !models.Any())
            {
                return NotFound("No models found.");
            }

            return Ok(models);
        }
    }
}
