using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Services
{
    public class ChatHistoryService : IChatHistoryService
    {
        private readonly IChatHistoryDataContext _chatHistoryDataContext;

        public ChatHistoryService(IChatHistoryDataContext chatHistoryDataContext)
        {
            _chatHistoryDataContext = chatHistoryDataContext;
        }

        // Fetch chat sessions and messages by UserId and ModelId
        public ChatSession GetHistory(Guid userId, Guid chatSessionIdentity)
        {
            var session = _chatHistoryDataContext.GetHistory(userId, chatSessionIdentity);

            if (session != null)
            {
                return session;
            }
            else
            {
                return new ChatSession
                {
                    UniqueIdentity = chatSessionIdentity,
                    UserId = userId,
                    Messages = new List<ChatMessage>(),
                    CreatedAt = DateTime.UtcNow
                };
            }
        }

        // Save history (add messages to a session)
        public void SaveHistory(Guid userId, List<ChatMessage> messages)
        {
            _chatHistoryDataContext.SaveHistory(userId, messages);
        }
    }
}