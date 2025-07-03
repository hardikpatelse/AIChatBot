using AIChatBot.API.Factory;
using AIChatBot.API.Interfaces;
using AIChatBot.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly RetryFileOperationService _retryService;
        private readonly ChatHistoryService _chatHistoryService;
        private readonly ChatModelServiceFactory _factory;
        private readonly AgentService _agentService;
        private readonly ToolsRegistryService _toolsRegistryService;

        public ChatService(RetryFileOperationService retryService,
            ChatHistoryService chatHistoryService,
            ChatModelServiceFactory factory,
            AgentService agentService,
            ToolsRegistryService toolsRegistryService)
        {
            _retryService = retryService;
            _chatHistoryService = chatHistoryService;
            _factory = factory;
            _agentService = agentService;
            _toolsRegistryService = toolsRegistryService;
        }

        public IActionResult GetHistory(string modelId)
        {
            var modelHistory = _retryService.RetryFileOperation(() =>
            {
                var history = _chatHistoryService.GetHistory(modelId);
                return new OkObjectResult(history);
            });

            return modelHistory;
        }

        public async Task<IActionResult> PostChat(ChatRequest request)
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
                var prompt = preparePrompt(request.Message);
                var service = _factory.GetService(request.Model);
                var aiResponse = await service.SendMessageAsync(request.Model, prompt);
                responseText = await _agentService.RunToolAsync(aiResponse);
            }
            else if (request.AIMode == "agent")
            {
                var service = _factory.GetService(request.Model);
                var functions = _toolsRegistryService.GetToolSchemas();
                var response = await service.ChatWithFunctionSupportAsync(request.Model, request.Message, functions);
                responseText = await _agentService.RunAgentAsync(response);
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
            return new OkObjectResult(new ChatResponse { Response = responseText });
        }

        private string preparePrompt(string userInput)
        {
            // Construct prompt with tools
            var toolList = "Tools: CreateFile, FetchWebData, SendEmail\n";
            var systemPrompt = $"{toolList}\nUnderstand user's intent and call one tool. Provide me the response in json string with properties 'tool' & 'parameters', and in 'parameters', respective properties will be available. Do not include any code expressions.";

            return $"{systemPrompt}\nUser: {userInput}";
        }
    }
}
