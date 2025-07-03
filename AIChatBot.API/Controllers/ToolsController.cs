using AIChatBot.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly ToolsRegistryService _toolsRegistryService;
        public ToolsController(ToolsRegistryService toolsRegistryService)
        {
            _toolsRegistryService = toolsRegistryService;
        }

        [HttpGet("schema")]
        public IActionResult GetSchemas()
        {
            return Ok(_toolsRegistryService.GetToolSchemas());
        }
    }
}
