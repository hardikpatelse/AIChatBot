using AIChatBot.API.Factory;
using AIChatBot.API.Models;
using System.Text.Json;

namespace AIChatBot.API.Services
{
    public class AgentService
    {
        private readonly ChatModelServiceFactory _factory;

        public AgentService(ChatModelServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> RunToolAsync(string aiResponse)
        {
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
                    switch (tool)
                    {
                        case "CreateFile":
                            var filename = parametersElement.GetProperty("filename").GetString();
                            var content = parametersElement.GetProperty("content").GetString();
                            return ToolFunctions.CreateFile(filename, content);

                        case "FetchWebData":
                            var url = parametersElement.GetProperty("url").GetString();
                            return ToolFunctions.FetchWebData(url);

                        case "SendEmail":
                            var to = parametersElement.GetProperty("to").GetString();
                            var subject = parametersElement.GetProperty("subject").GetString();
                            var body = parametersElement.GetProperty("body").GetString();
                            return ToolFunctions.SendEmail(to, subject, body);
                    }
                }
            }
            catch (JsonException)
            {
                // Fallback to legacy parsing if not valid JSON
            }

            //// Legacy parsing (if not JSON)
            //if (response.Contains("CreateFile"))
            //{
            //    var match = Regex.Match(response, @"CreateFile\(([^,]+),\s?(.+)\)");
            //    return ToolFunctions.CreateFile(match.Groups[1].Value.Trim('"'), match.Groups[2].Value.Trim('"'));
            //}
            //if (response.Contains("FetchWebData"))
            //{
            //    var match = Regex.Match(response, @"FetchWebData\((.+)\)");
            //    return ToolFunctions.FetchWebData(match.Groups[1].Value.Trim('"'));
            //}
            //if (response.Contains("SendEmail"))
            //{
            //    var match = Regex.Match(response, @"SendEmail\(([^,]+),\s?([^,]+),\s?(.+)\)");
            //    return ToolFunctions.SendEmail(match.Groups[1].Value.Trim('"'), match.Groups[2].Value.Trim('"'), match.Groups[3].Value.Trim('"'));
            //}

            return "🤖 No matching tool found.";
        }

        public async Task<string> RunAgentAsync(List<FunctionCallResult> functionCallResults)
        {
            var result = "";

            foreach (var functionCall in functionCallResults)
            {
                if (!string.IsNullOrEmpty(functionCall.FunctionName) && !string.IsNullOrEmpty(functionCall.ArgumentsJson))
                {
                    try
                    {
                        var args = JsonDocument.Parse(functionCall.ArgumentsJson).RootElement;
                        if (ToolMap.TryGetValue(functionCall.FunctionName, out var func))
                        {
                            var toolResult = func(args);
                            return $"✅ Tool `{functionCall.FunctionName}` executed:\n{toolResult}";
                        }
                        return $"❌ Unknown tool: {functionCall.FunctionName}";
                    }
                    catch (Exception ex)
                    {
                        return $"❌ Error executing tool `{functionCall.FunctionName}`: {ex.Message}";
                    }
                }

            }
            return  result ?? "🤖 No tool used. Here's my response.";
        }

        private static readonly Dictionary<string, Func<JsonElement, string>> ToolMap = new()
        {
            ["CreateFile"] = args => ToolFunctions.CreateFile(
                args.GetProperty("filename").GetString()!,
                args.GetProperty("content").GetString()!
            ),
            ["FetchWebData"] = args => ToolFunctions.FetchWebData(
                args.GetProperty("url").GetString()!
            ),
            ["SendEmail"] = args => ToolFunctions.SendEmail(
                args.GetProperty("to").GetString()!,
                args.GetProperty("subject").GetString()!,
                args.GetProperty("body").GetString()!
            )
        };

    }

}
