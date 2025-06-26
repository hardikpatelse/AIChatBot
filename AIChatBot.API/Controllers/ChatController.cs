using AIChatBot.API.Factory;
using AIChatBot.API.Models;
using AIChatBot.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly RetryFileOperationService _retryService;
        private readonly ChatModelServiceFactory _factory;
        private readonly ChatHistoryService _chatHistoryService;
        private readonly AgentService _agentService;

        public ChatController(
            RetryFileOperationService retryService,
            ChatModelServiceFactory factory,
            ChatHistoryService chatHistoryService,
            AgentService agentService)
        {
            _retryService = retryService;
            _factory = factory;
            _chatHistoryService = chatHistoryService;
            _agentService = agentService;
        }

        [HttpGet("history")]
        public IActionResult GetHistory([FromQuery] string modelId)
        {
            return _retryService.RetryFileOperation(() =>
            {
                var modelHistory = _chatHistoryService.GetHistory(modelId);
                return Ok(modelHistory);
            }, this);
        }

        [HttpPost]
        public async Task<IActionResult> PostChat([FromBody] ChatRequest request)
        {
            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Role = "user",
                    Content = request.Message,
                    DateTime = DateTime.UtcNow
                }
            };

            _chatHistoryService.SaveHistory(request.Model, messages);

            string responseText;
            
            if (request.AIMode == "tools")
            {
                responseText = await _agentService.RunAgentAsync(request.Model, request.Message);
            }
            else
            {
                var service = _factory.GetService(request.Model);
                responseText = await service.SendMessageAsync(request.Model, request.Message);
            }

            messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Role = "assistant",
                    Content = responseText,
                    DateTime = DateTime.UtcNow
                }
            };

            _chatHistoryService.SaveHistory(request.Model, messages);
            return Ok(new ChatResponse { Response = responseText });
        }
    }
}
