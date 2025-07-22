using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.DataContext
{
    public interface IChatHistoryDataContext
    {
        ChatSession? GetHistory(Guid userId, Guid chatSessionIdentity, int modelId);
        void SaveHistory(Guid userId, int chatSessionId, List<ChatMessage> messages);
    }
}
