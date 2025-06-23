using AIChatBot.API.Interfaces;
using AIChatBot.API.Models;
using Microsoft.Extensions.Logging;
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

        public OllamaChatService(HttpClient httpClient, ILogger<OllamaChatService> logger, IOptions<OllamaModelsApi> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configurations = options.Value;
        }

        public async Task<string> SendMessageAsync(string model, string message)
        {
            try
            {
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

                return ConvertOllamaResponse(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in OllamaChatService.SendMessageAsync");
                throw;
            }
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
