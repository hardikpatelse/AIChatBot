using AIChatBot.API.Factory;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models;
using AIChatBot.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIChatBot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatHistoryService _chatHistoryService;
        private readonly ChatModelServiceFactory _factory;
        private readonly AgentService _agentService;
        private readonly IChatSessionServices _chatSessionServices;
        private readonly IModelService _modelService;

        public ChatService(
            IChatHistoryService chatHistoryService,
            ChatModelServiceFactory factory,
            AgentService agentService,
            IChatSessionServices chatSessionServices,
            IModelService modelService)
        {
            _chatHistoryService = chatHistoryService;
            _factory = factory;
            _agentService = agentService;
            _chatSessionServices = chatSessionServices;
            _modelService = modelService;
        }

        public IActionResult GetHistory(Guid userId, Guid chatSessionIdentity)
        {
            var history = _chatHistoryService.GetHistory(userId, chatSessionIdentity);
            return new OkObjectResult(history);
        }

        public async Task<IActionResult> PostChat(ChatRequest request)
        {
            var sessionWithoutMessages = await _chatSessionServices.GetSessionWithoutMessages(request.UserId, request.ChatSessionIdentity);
            var selectedModel = _modelService.GetModelById(request.ModelId);
            // Validate request
            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Role = "user",
                    ChatSessionId = sessionWithoutMessages.Id,
                    Content = request.Message,
                    TimeStamp = DateTime.UtcNow
                }
            };

            _chatHistoryService.SaveHistory(request.UserId, sessionWithoutMessages.Id, messages);

            string responseText = string.Empty;
            bool saveHistory = true;

            if (request.AIMode == "tools")
            {
                var prompt = preparePrompt(request.Message);
                var service = _factory.GetService(selectedModel.ModelName);
                var aiResponse = await service.SendMessageAsync(selectedModel.ModelName, prompt);
                responseText = await _agentService.RunToolAsync(aiResponse);
            }
            else if (request.AIMode == "agent")
            {
                var service = _factory.GetService(selectedModel.ModelName);

                var msgObject = new List<Dictionary<string, string>>
                {
                    new() { ["role"] = "user", ["content"] = request.Message }
                };
                var response = await service.ChatWithFunctionSupportAsync(selectedModel.ModelName, msgObject);
                responseText = await _agentService.RunAgentAsync(response);
            }
            else if (request.AIMode == "planner")
            {
                var service = _factory.GetService(selectedModel.ModelName);
                var funcExecLog = new List<FunctionCallResult>();
                for (int step = 0; step < 5; step++) // Limit recursion
                {
                    var msgObject = preparePlannerPrompt(request, step, funcExecLog);

                    //Serialize the message object for logging
                    var msgJson = JsonSerializer.Serialize(msgObject, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });

                    var response = await service.ChatWithFunctionSupportAsync(selectedModel.ModelName, msgObject);

                    var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                    Console.WriteLine(responseJson);
                    if (!response.Any(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                    {
                        break;
                    }
                    var iterationResponse = await _agentService.RunAgentAsync(response);
                    foreach (var funcCall in response.Where(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                    {
                        funcExecLog.Add(new FunctionCallResult()
                        {
                            FunctionName = funcCall.FunctionName,
                            ArgumentsJson = funcCall.ArgumentsJson,
                            TextResponse = iterationResponse
                        });
                    }
                    messages = new List<ChatMessage>
                    {
                        new ChatMessage
                        {
                            Role = "assistant",
                            Content = iterationResponse,
                            TimeStamp = DateTime.UtcNow
                        }
                    };

                    _chatHistoryService.SaveHistory(request.UserId, sessionWithoutMessages.Id, messages);

                    responseText += $"Recursion Step: {step} \n" + iterationResponse;
                }
                saveHistory = false; // Don't save history for planner mode
                if (string.IsNullOrWhiteSpace(responseText))
                {
                    responseText = "⚠️ Agent loop ended without clear conclusion.";
                }
            }
            else
            {
                var service = _factory.GetService(selectedModel.ModelName);
                responseText = await service.SendMessageAsync(selectedModel.ModelName, request.Message);
            }

            if (saveHistory)
            {
                messages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        Role = "assistant",
                        Content = responseText,
                        TimeStamp = DateTime.UtcNow
                    }
                };

                _chatHistoryService.SaveHistory(request.UserId, sessionWithoutMessages.Id, messages);
            }
            return new OkObjectResult(new ChatResponse { Response = responseText });
        }

        private string preparePrompt(string userInput)
        {
            // Construct prompt with tools
            var toolList = "Tools: CreateFile, FetchWebData, SendEmail\n";
            var systemPrompt = $"{toolList}\nUnderstand user's intent and call one tool. Provide me the response in json string with properties 'tool' & 'parameters', and in 'parameters', respective properties will be available. Do not include any code expressions.";

            return $"{systemPrompt}\nUser: {userInput}";
        }

        private List<Dictionary<string, string>> preparePlannerPrompt(ChatRequest request, int step, List<FunctionCallResult> functionCalls)
        {
            var chatSession = _chatHistoryService.GetHistory(request.UserId, request.ChatSessionIdentity);

            var msgObject = chatSession.Messages.Select(m => new Dictionary<string, string>
            {
                ["role"] = m.Role.ToLower(),  // "user" or "assistant"
                ["content"] = m.Content
            }).ToList();

            if (msgObject.Count == 0 || step == 0)
            {
                // If no history, start with the user message
                if (msgObject.Count == 0)
                {
                    msgObject = new List<Dictionary<string, string>>();
                }
                msgObject.Add(new Dictionary<string, string>
                {
                    ["role"] = "system", 
                    ["content"] = "You are a tool-using agent. When your task is complete, end with a natural sentence and do not call a function."
                });
                msgObject.Add(new Dictionary<string, string>
                {
                    ["role"] = "user", 
                    ["content"] = request.Message
                });
            }
            else
            {
                foreach (var item in functionCalls.Where(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                {
                    msgObject.Add(new Dictionary<string, string>
                    {
                        ["role"] = "function",
                        ["name"] = item.FunctionName,
                        ["content"] = item.TextResponse
                    });
                }
                msgObject.Add(new Dictionary<string, string>
                {
                    ["role"] = "user",
                    ["content"] = "Continue"
                });
            }

            return msgObject;
        }
    }
}
