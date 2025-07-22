using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Requests;
using System.Threading.Tasks;

namespace AIChatBot.API.Interfaces.DataContext
{
    public interface IChatSessionDataContext
    {
        Task<List<ChatSession>> GetSessionsWithoutMessages(Guid userId);
        Task<bool> RenameChatSessionAsync(ChatSessionRequest request);
        Task<ChatSession> CreateSessionAsync(ChatSessionRequest request);
    }
}
