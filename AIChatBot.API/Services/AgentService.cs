using AIChatBot.API.AIServices;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace AIChatBot.API.Services
{
    public class AgentService
    {
        private readonly OpenRouterChatService _model;

        public AgentService(OpenRouterChatService model)
        {
            _model = model;
        }

        public async Task<string> RunAgentAsync(string model, string userInput)
        {
            // Construct prompt with tools
            var toolList = "Tools: CreateFile, FetchWebData, SendEmail\n";
            var systemPrompt = $"{toolList}\nUnderstand user's intent and call one tool. Provide me the response in json string. Do not include any code expressions.";

            var fullPrompt = $"{systemPrompt}\nUser: {userInput}";

            var response = await _model.SendMessageAsync(model, fullPrompt);

            // Remove code block formatting if present (e.g., ```json ... ```)
            response = response.Trim();
            if (response.StartsWith("```"))
            {
                var firstNewline = response.IndexOf('\n');
                if (firstNewline != -1)
                {
                    // Remove the opening ```
                    response = response.Substring(firstNewline + 1);
                    // Remove the closing ```
                    var lastCodeBlock = response.LastIndexOf("```", StringComparison.Ordinal);
                    if (lastCodeBlock != -1)
                    {
                        response = response.Substring(0, lastCodeBlock);
                    }
                }
                response = response.Trim();
            }

            // Try to parse JSON response
            try
            {
                using var doc = JsonDocument.Parse(response);
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
    }

}
