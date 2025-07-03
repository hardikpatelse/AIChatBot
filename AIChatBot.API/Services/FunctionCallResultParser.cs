using AIChatBot.API.Models;
using AIChatBot.API.Models.ToolResponse;
using System.Text.Json;

namespace AIChatBot.API.Services
{
    public static class FunctionCallResultParser
    {
        public static List<FunctionCallResult> ParseFunctionCallResults(string json)
        {
            var results = new List<FunctionCallResult>();
            var root = JsonSerializer.Deserialize<AIResponse>(json.Trim());

            if (root?.choices != null)
            {
                foreach (var choice in root.choices)
                {
                    var toolCalls = choice.message?.tool_calls;
                    if (toolCalls != null)
                    {
                        foreach (var tool in toolCalls)
                        {
                            results.Add(new FunctionCallResult
                            {
                                FunctionName = tool.function?.name,
                                ArgumentsJson = tool.function?.arguments,
                                TextResponse = null
                            });
                        }
                    }
                }
            }

            return results;
        }
    }

}
