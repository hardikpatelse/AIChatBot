using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Requests;
using System.Threading.Tasks;

namespace AIChatBot.API.Interfaces.Services
{
    public interface IChatSessionServices
    {
        Task<List<ChatSession>> GetChatSessionsWithoutMessages(Guid userId);
        Task<ChatSession> GetSessionWithoutMessages(Guid userId, Guid chatSessionIdentity);
        Task<bool> RenameChatSessionAsync(ChatSessionRequest request);
        Task<ChatSession> CreateSessionAsync(ChatSessionRequest request);
    }
}
