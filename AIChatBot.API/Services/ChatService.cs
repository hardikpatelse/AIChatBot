using AIChatBot.API.Factory;
using AIChatBot.API.Interfaces;
using AIChatBot.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIChatBot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatHistoryService _chatHistoryService;
        private readonly ChatModelServiceFactory _factory;
        private readonly AgentService _agentService;

        public ChatService(
            ChatHistoryService chatHistoryService,
            ChatModelServiceFactory factory,
            AgentService agentService)
        {
            _chatHistoryService = chatHistoryService;
            _factory = factory;
            _agentService = agentService;
        }

        public IActionResult GetHistory(string modelId)
        {
            var history = _chatHistoryService.GetHistory(modelId);
            return new OkObjectResult(history);
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

            string responseText = string.Empty;
            bool saveHistory = true;

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

                var msgObject = new List<Dictionary<string, string>>
                {
                    new() { ["role"] = "user", ["content"] = request.Message }
                };
                var response = await service.ChatWithFunctionSupportAsync(request.Model, msgObject);
                responseText = await _agentService.RunAgentAsync(response);
            }
            else if (request.AIMode == "planner")
            {
                var service = _factory.GetService(request.Model);
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
                    Console.WriteLine(msgJson);

                    var response = await service.ChatWithFunctionSupportAsync(request.Model, msgObject);


                    var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                    Console.WriteLine(responseJson);
                    if (!response.Any(a=> !string.IsNullOrWhiteSpace(a.FunctionName)))
                    {
                        break;
                    }
                    var iterationResponse = await _agentService.RunAgentAsync(response);
                    foreach (var funcCall in response.Where(a => !string.IsNullOrWhiteSpace(a.FunctionName)))
                    {
                        funcExecLog.Add(new FunctionCallResult() { 
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
                            DateTime = DateTime.UtcNow
                        }
                    };

                    _chatHistoryService.SaveHistory(request.Model, messages);

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
                var service = _factory.GetService(request.Model);
                responseText = await service.SendMessageAsync(request.Model, request.Message);
            }

            if (saveHistory)
            {
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
            var history = _chatHistoryService.GetHistory(request.Model);

            var msgObject = history.History.Select(m => new Dictionary<string, string>
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
                msgObject.AddRange(
                    new() { ["role"] = "system", ["content"] = "You are a tool-using agent. When your task is complete, end with a natural sentence and do not call a function." },
                    new() { ["role"] = "user", ["content"] = request.Message }
                    );
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
