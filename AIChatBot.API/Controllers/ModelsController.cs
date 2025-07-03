using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AIChatBot.API.Services;
using AIChatBot.API.Models;

namespace AIChatBot.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly RetryFileOperationService _retryService;

        public ModelsController(RetryFileOperationService retryService)
        {
            _retryService = retryService;
        }

        [HttpGet]
        public IActionResult GetModels()
        {
            return _retryService.RetryFileOperation(() =>
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var models = JsonSerializer.Deserialize<List<ModelResponse>>(
                    System.IO.File.ReadAllText("Data/models.json"), options);
                return Ok(models);
            });
        }
    }
}
