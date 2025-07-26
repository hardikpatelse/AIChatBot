using AIChatBot.API.Hubs;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace AIChatBot.API.Services
{
    public class AgentService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFileService _fileService;

        public AgentService(IHubContext<ChatHub> hubContext, IFileService fileService)
        {
            _hubContext = hubContext;
            _fileService = fileService;
        }

        public async Task<string> RunToolAsync(string aiResponse, Guid userId, int chatSessionId, string? connectionId = null)
        {
            // Broadcast thinking status
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "🟡 Thinking...");
            }
            // Remove code block formatting if present (e.g., ```json ... ```)
            aiResponse = aiResponse.Trim();
            if (aiResponse.StartsWith("```"))
            {
                var firstNewline = aiResponse.IndexOf('\n');
                if (firstNewline != -1)
                {
                    // Remove the opening ```
                    aiResponse = aiResponse.Substring(firstNewline + 1);
                    // Remove the closing ```
                    var lastCodeBlock = aiResponse.LastIndexOf("```", StringComparison.Ordinal);
                    if (lastCodeBlock != -1)
                    {
                        aiResponse = aiResponse.Substring(0, lastCodeBlock);
                    }
                }
                aiResponse = aiResponse.Trim();
            }

            // Try to parse JSON response
            try
            {
                using var doc = JsonDocument.Parse(aiResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("tool", out var toolElement) &&
                    root.TryGetProperty("parameters", out var parametersElement))
                {
                    var tool = toolElement.GetString();
                    
                    // Broadcast executing status
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", $"🟢 Executing tool {tool}");
                    }

                    string result;
                    switch (tool)
                    {
                        case "CreateFile":
                            var filename = parametersElement.GetProperty("filename").GetString()!;
                            var content = parametersElement.GetProperty("content").GetString()!;
                            result = await _fileService.CreateFileAsync(filename, content, userId, chatSessionId);
                            break;

                        case "FetchWebData":
                            var url = parametersElement.GetProperty("url").GetString();
                            result = ToolFunctions.FetchWebData(url);
                            break;

                        case "SendEmail":
                            var to = parametersElement.GetProperty("to").GetString();
                            var subject = parametersElement.GetProperty("subject").GetString();
                            var body = parametersElement.GetProperty("body").GetString();
                            result = ToolFunctions.SendEmail(to, subject, body);
                            break;
                            
                        default:
                            result = "🤖 No matching tool found.";
                            break;
                    }

                    // Broadcast completion status
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "✅ Completed!");
                    }

                    return result;
                }
            }
            catch (JsonException)
            {
                // Fallback to legacy parsing if not valid JSON
            }

            return "🤖 No matching tool found.";
        }

        public async Task<string> RunAgentAsync(List<FunctionCallResult> functionCallResults, Guid userId, int chatSessionId, string? connectionId = null)
        {
            var result = "";

            // Broadcast executing status

            foreach (var functionCall in functionCallResults)
            {
                if (!string.IsNullOrEmpty(connectionId) && functionCallResults.Any())
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", $"🟢 Executing tool {functionCall.FunctionName}");
                }
                if (!string.IsNullOrEmpty(functionCall.FunctionName) && !string.IsNullOrEmpty(functionCall.ArgumentsJson))
                {
                    try
                    {
                        var args = JsonDocument.Parse(functionCall.ArgumentsJson).RootElement;
                        string toolResult;
                        
                        switch (functionCall.FunctionName)
                        {
                            case "CreateFile":
                                var filename = args.GetProperty("filename").GetString()!;
                                var content = args.GetProperty("content").GetString()!;
                                toolResult = await _fileService.CreateFileAsync(filename, content, userId, chatSessionId);
                                break;
                            case "FetchWebData":
                                var url = args.GetProperty("url").GetString()!;
                                toolResult = ToolFunctions.FetchWebData(url);
                                break;
                            case "SendEmail":
                                var to = args.GetProperty("to").GetString()!;
                                var subject = args.GetProperty("subject").GetString()!;
                                var body = args.GetProperty("body").GetString()!;
                                toolResult = ToolFunctions.SendEmail(to, subject, body);
                                break;
                            default:
                                toolResult = $"❌ Unknown tool: {functionCall.FunctionName}";
                                break;
                        }
                        
                        result += $"✅ Tool `{functionCall.FunctionName}` executed:\n{toolResult}\n";
                    }
                    catch (Exception ex)
                    {
                        return $"❌ Error executing tool `{functionCall.FunctionName}`: {ex.Message}";
                    }
                }
                else if (!string.IsNullOrEmpty(functionCall.TextResponse))
                {
                    return $"🤖 Final Response:\n{functionCall.TextResponse}";
                }
            }

            // Broadcast completion status
            if (!string.IsNullOrEmpty(connectionId) && functionCallResults.Any())
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "✅ Completed!");
            }

            return result ?? "🤖 No tool used. Here's my response.";
        }



    }

}
