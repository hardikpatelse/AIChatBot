using AIChatBot.API.Interfaces;
using AIChatBot.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("history")]
        public IActionResult GetHistory([FromQuery] string modelId)
        {
            return _chatService.GetHistory(modelId);
        }

        [HttpPost]
        public async Task<IActionResult> PostChat([FromBody] ChatRequest request)
        {
            return await _chatService.PostChat(request);
        }
    }
}
