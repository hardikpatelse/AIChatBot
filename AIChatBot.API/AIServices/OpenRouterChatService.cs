using AIChatBot.API.Interfaces;
using AIChatBot.API.Models;
using AIChatBot.API.Models.Tools_Structure;
using AIChatBot.API.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIChatBot.API.AIServices
{
    public class OpenRouterChatService : IChatModelService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenRouterChatService> _logger;
        private readonly OpenRouterModelsApi _configurations;

        public OpenRouterChatService(HttpClient httpClient, ILogger<OpenRouterChatService> logger, IOptions<OpenRouterModelsApi> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configurations = options.Value;
        }

        public async Task<string> SendMessageAsync(string model, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_configurations.Url))
                    throw new InvalidOperationException("OpenRouter URL is not configured.");

                if (string.IsNullOrWhiteSpace(_configurations.ApiKey))
                    throw new InvalidOperationException("OpenRouter API key is not configured.");

                var requestData = new
                {
                    model = model,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = message
                        }
                    }
                };

                var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, _configurations.Url)
                {
                    Content = requestContent
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configurations.ApiKey);

                _httpClient.Timeout = TimeSpan.FromMinutes(5);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStringAsync();

                return ConvertDeepseekResponse(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in OpenRouterChatService.SendMessageAsync");
                throw;
            }
        }

        public async Task<List<FunctionCallResult>> ChatWithFunctionSupportAsync(string model, string userMessage, List<ToolDefinition> tools)
        {
            var functionCallResults = new List<FunctionCallResult>();
            try
            {
                var apiKey = _configurations.ApiKey;

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = userMessage
                        }
                    },
                    tools = tools,
                    tool_choice = "auto" // Let the model decide
                };

                var request = new HttpRequestMessage(HttpMethod.Post, _configurations.Url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                functionCallResults = FunctionCallResultParser.ParseFunctionCallResults(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in OpenRouterChatService.ChatWithFunctionSupportAsync");
                throw;
            }

            return functionCallResults;
        }

        private string ConvertDeepseekResponse(string modelResponse)
        {
            try
            {
                using var doc = JsonDocument.Parse(modelResponse);
                if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.ValueKind == JsonValueKind.Array &&
                    choices.GetArrayLength() > 0)
                {
                    var firstChoice = choices[0];
                    if (firstChoice.TryGetProperty("message", out var message) &&
                        message.TryGetProperty("content", out var content))
                    {
                        return content.GetString() ?? string.Empty;
                    }
                }
            }
            catch (JsonException)
            {
                // Optionally log or handle malformed JSON
            }
            return string.Empty;
        }
    }
}
