using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.DataContext
{
    public class ChatHistoryDataContext : IChatHistoryDataContext
    {
        private readonly ChatBotDbContext _dbContext;

        public ChatHistoryDataContext(ChatBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ChatSession? GetHistory(Guid userId, Guid chatSessionIdentity, int modelId)
        {
            return _dbContext.ChatSessions
                .Include(s => s.Messages)
                .Where(s => s.UserId == userId && s.UniqueIdentity == chatSessionIdentity && s.ModelId == modelId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefault();

        }

        public void SaveHistory(Guid userId, int chatSessionId, List<ChatMessage> messages)
        {
            var session = _dbContext.ChatSessions.Include(s => s.Messages).FirstOrDefault(s => s.Id == chatSessionId);
            if (session != null)
            {
                foreach (var msg in messages)
                {
                    msg.ChatSessionId = chatSessionId;
                    _dbContext.ChatMessages.Add(msg);
                }
                _dbContext.SaveChanges();
            }
        }
    }
}
