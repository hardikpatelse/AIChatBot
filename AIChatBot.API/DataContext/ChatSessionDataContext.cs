using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.DataContext
{
    public class ChatSessionDataContext : IChatSessionDataContext
    {
        private readonly ChatBotDbContext _dbContext;

        public ChatSessionDataContext(ChatBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ChatSession>> GetSessionsWithoutMessages(Guid userId)
        {
            return await _dbContext.ChatSessions.Where(s=>s.UserId == userId).ToListAsync();
        }

        public async Task<bool> RenameChatSessionAsync(ChatSessionRequest request)
        {
            var session = await _dbContext.ChatSessions.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (session == null)
                return false;
            session.Name = request.Name;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ChatSession> CreateSessionAsync(ChatSessionRequest request)
        {
            var session = new ChatSession
            {
                Name = request.Name,
                UserId = request.UserId.Value,
                UniqueIdentity = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Messages = new List<ChatMessage>()
            };
            _dbContext.ChatSessions.Add(session);
            await _dbContext.SaveChangesAsync();
            return session;
        }
    }
}
