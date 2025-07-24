using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.Services
{
    public interface IChatHistoryService
    {
        ChatSession GetHistory(Guid userId, Guid chatSessionIdentity);
        void SaveHistory(Guid userId, int chatSessionId, List<ChatMessage> messages);
    }
}
