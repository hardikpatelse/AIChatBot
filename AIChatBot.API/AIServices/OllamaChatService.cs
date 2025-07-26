using AIChatBot.API.Hubs;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models;
using AIChatBot.API.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AIChatBot.API.AIServices
{
    public class OllamaChatService : IChatModelService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OllamaChatService> _logger;
        private readonly OllamaModelsApi _configurations;
        private readonly ToolsRegistryService _toolsRegistryService;
        private readonly IHubContext<ChatHub> _hubContext;

        public OllamaChatService(HttpClient httpClient, ILogger<OllamaChatService> logger, IOptions<OllamaModelsApi> options, ToolsRegistryService toolsRegistryService, IHubContext<ChatHub> hubContext)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configurations = options.Value;
            _toolsRegistryService = toolsRegistryService;
            _hubContext = hubContext;
        }

        public async Task<string> SendMessageAsync(string model, string message, string connectionId)
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "🟡 Thinking...");
                }

                var requestData = new
                {
                    model = model,
                    prompt = message
                };

                var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                _httpClient.Timeout = TimeSpan.FromMinutes(5);
                var response = await _httpClient.PostAsync(_configurations.Url, requestContent);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "🟡 Analyzing...");
                }
                return ConvertOllamaResponse(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in OllamaChatService.SendMessageAsync");
                throw;
            }
        }

        public async Task<List<FunctionCallResult>> ChatWithFunctionSupportAsync(string model, List<Dictionary<string, string>> userMessage, string connectionId)
        {
            var tools = _toolsRegistryService.GetToolSchemas();
            var functionCallResults = new List<FunctionCallResult>();
            try
            {
                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "🟡 Thinking...");
                }
                var requestBody = new
                {
                    model = model,
                    prompt = userMessage,
                    tools = tools,
                    tool_choice = "auto" // Let the model decide
                };

                var request = new HttpRequestMessage(HttpMethod.Post, _configurations.Url);
                request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                _httpClient.Timeout = TimeSpan.FromMinutes(5);

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveStatus", "🟡 Analyzing...");
                }

                functionCallResults = FunctionCallResultParser.ParseFunctionCallResults(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in OllamaChatService.ChatWithFunctionSupportAsync");
                throw;
            }
            // Ensure all code paths return a value
            return functionCallResults;
        }

        private string ConvertOllamaResponse(string modelResponse)
        {
            var sb = new StringBuilder();
            var lines = modelResponse.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                try
                {
                    using var doc = JsonDocument.Parse(line);
                    if (doc.RootElement.TryGetProperty("response", out var responseProp))
                    {
                        var part = responseProp.GetString();
                        if (!string.IsNullOrEmpty(part))
                            sb.Append(part);
                    }
                }
                catch (JsonException)
                {
                    continue;
                }
            }

            return sb.ToString();
        }
    }
}
