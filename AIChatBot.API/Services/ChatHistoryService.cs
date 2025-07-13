using AIChatBot.API.Models;
using AIChatBot.API.Models.Custom;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AIChatBot.API.Services
{
    public class ChatHistoryService
    {
        private readonly string _chatHistoryPath = "Data/chatHistory.json";
        private readonly int _retryCount;
        private readonly int _retryDelayMs;

        public ChatHistoryService(IOptions<ChatHistoryOptions> options)
        {
            var opts = options.Value;
            _retryCount = opts.RetryCount;
            _retryDelayMs = opts.RetryDelayMilliseconds;
        }

        public ModelChatHistory GetHistory(string modelId)
        {
            int retries = _retryCount;
            while (retries-- > 0)
            {
                try
                {
                    if (!File.Exists(_chatHistoryPath))
                        return new ModelChatHistory { ModelId = modelId, History = new List<ChatMessage>() };

                    var allHistories = JsonSerializer.Deserialize<List<ModelChatHistory>>(
                        File.ReadAllText(_chatHistoryPath),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ModelChatHistory>();

                    var modelHistory = allHistories.FirstOrDefault(h => h.ModelId == modelId);
                    if (modelHistory == null)
                        return new ModelChatHistory { ModelId = modelId, History = new List<ChatMessage>() };

                    return modelHistory;
                }
                catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32 && retries > 0)
                {
                    Thread.Sleep(_retryDelayMs);
                }
            }
            // If all retries fail, return empty history
            return new ModelChatHistory { ModelId = modelId, History = new List<ChatMessage>() };
        }

        public void SaveHistory(string modelId, List<ChatMessage> histories)
        {
            int retries = _retryCount;
            while (retries-- > 0)
            {
                try
                {
                    List<ModelChatHistory> allHistories;
                    if (File.Exists(_chatHistoryPath))
                    {
                        allHistories = JsonSerializer.Deserialize<List<ModelChatHistory>>(
                            File.ReadAllText(_chatHistoryPath),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ModelChatHistory>();
                    }
                    else
                    {
                        allHistories = new List<ModelChatHistory>();
                    }

                    var modelHistory = allHistories.FirstOrDefault(h => h.ModelId == modelId);
                    if (modelHistory == null)
                    {
                        modelHistory = new ModelChatHistory { ModelId = modelId, History = new List<ChatMessage>() };
                        allHistories.Add(modelHistory);
                    }

                    modelHistory.History.AddRange(histories);

                    File.WriteAllText(_chatHistoryPath, JsonSerializer.Serialize(allHistories, new JsonSerializerOptions { WriteIndented = true }));
                    break;
                }
                catch when (retries > 0)
                {
                    Thread.Sleep(_retryDelayMs);
                }
            }
        }
    }
}