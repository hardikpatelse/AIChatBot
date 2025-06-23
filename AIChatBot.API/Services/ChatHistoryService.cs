using AIChatBot.API.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AIChatBot.API.Services
{
    public class ChatHistoryService
    {
        private readonly string _chatHistoryPath = "Data/chatHistory.json";
        private readonly RetryFileOperationService _retryService;

        public ChatHistoryService(RetryFileOperationService retryService)
        {
            _retryService = retryService;
        }

        public ModelChatHistory GetHistory(string modelId)
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

        public void SaveHistory(string modelId, List<ChatMessage> histories)
        {
            int retries = 3;
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
                    System.Threading.Thread.Sleep(200);
                }
            }
        }
    }
}