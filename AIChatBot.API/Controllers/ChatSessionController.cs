using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatSessionController : ControllerBase
    {
        private readonly IChatSessionServices _chatSessionService;
        public ChatSessionController(IChatSessionServices chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }

        [HttpGet("Sessions")]
        public async Task<IActionResult> GetChatSessions([FromQuery] Guid userId)
        {
            var sessions = await _chatSessionService.GetChatSessionsWithoutMessages(userId);
            return Ok(sessions);
        }

        [HttpPost("Rename")]
        public async Task<IActionResult> RenameChatSession([FromBody] ChatSessionRequest request)
        {
            var result = await _chatSessionService.RenameChatSessionAsync(request);
            if (result)
                return Ok();
            return BadRequest("Invalid session ID or name.");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSession([FromBody] ChatSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Invalid input for session creation.");
            }
            var session = await _chatSessionService.CreateSessionAsync(request);
            if (session != null)
                return Ok(session);
            return BadRequest("Failed to create session.");
        }
    }
}
