using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Requests;
using Microsoft.Extensions.Caching.Memory;

namespace AIChatBot.API.Services
{
    public class ChatSessionServices : IChatSessionServices
    {
        private readonly IChatSessionDataContext _chatSessionDataContext;
        private readonly IMemoryCache _cache;
        private string cacheKey = $"ChatSession_";
        public ChatSessionServices(IChatSessionDataContext chatSessionDataContext, IMemoryCache cache)
        {
            _chatSessionDataContext = chatSessionDataContext;
            _cache = cache;
        }

        public Task<List<ChatSession>> GetChatSessionsWithoutMessages(Guid userId)
        {
            cacheKey = $"ChatSession_{userId}";
            if (_cache.TryGetValue(cacheKey, out List<ChatSession> cachedSessions))
            {
                return Task.FromResult(cachedSessions);
            }
            else 
            {
                return FetchSessionsFromDatabase(userId);
            }
        }

        public async Task<ChatSession> GetSessionWithoutMessages(Guid userId, Guid chatSessionIdentity)
        {
            cacheKey = $"ChatSession_{userId}";
            if (_cache.TryGetValue(cacheKey, out List<ChatSession> cachedSessions))
            {
                if (cachedSessions != null && cachedSessions.Any(s => s.UniqueIdentity == chatSessionIdentity))
                {
                    return cachedSessions.FirstOrDefault(s => s.UniqueIdentity == chatSessionIdentity);
                }
                else
                {
                    return await FetchSessionFromDatabase(userId, chatSessionIdentity);
                }
            }
            return await FetchSessionFromDatabase(userId, chatSessionIdentity);
        }

        public async Task<bool> RenameChatSessionAsync(ChatSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return false;
            return await _chatSessionDataContext.RenameChatSessionAsync(request);
        }

        public async Task<ChatSession> CreateSessionAsync(ChatSessionRequest request)
        {
            if (request.UserId == null || request.ModelId == null || string.IsNullOrWhiteSpace(request.Name))
                return null;
            return await _chatSessionDataContext.CreateSessionAsync(request);
        }

        private async Task<ChatSession> FetchSessionFromDatabase(Guid userId, Guid chatSessionIdentity)
        {
            var sessions = await FetchSessionsFromDatabase(userId);
            var session = sessions?.FirstOrDefault(s => s.UniqueIdentity == chatSessionIdentity);

            return session ?? new ChatSession
            {
                UniqueIdentity = chatSessionIdentity,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<ChatMessage>()
            };
        }

        private async Task<List<ChatSession>?> FetchSessionsFromDatabase(Guid userId)
        {
            var sessions = await _chatSessionDataContext.GetSessionsWithoutMessages(userId);

            if (sessions != null)
            {
                _cache.Set(cacheKey, sessions, TimeSpan.FromMinutes(30));
            }
            return sessions;
        }
    }
}
