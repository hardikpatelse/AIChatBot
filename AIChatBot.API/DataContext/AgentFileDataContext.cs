using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.DataContext
{
    public class AgentFileDataContext : IAgentFileDataContext
    {
        private readonly ChatBotDbContext _dbContext;

        public AgentFileDataContext(ChatBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AgentFile> CreateFileAsync(string fileName, string filePath, string downloadUrl, long fileSize, Guid userId, int chatSessionId)
        {
            var agentFile = new AgentFile
            {
                FileName = fileName,
                FilePath = filePath,
                DownloadUrl = downloadUrl,
                FileSize = fileSize,
                UserId = userId,
                ChatSessionId = chatSessionId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.AgentFiles.Add(agentFile);
            await _dbContext.SaveChangesAsync();
            return agentFile;
        }

        public async Task<List<AgentFile>> GetFilesBySessionAsync(int chatSessionId, Guid userId)
        {
            return await _dbContext.AgentFiles
                .Where(af => af.ChatSessionId == chatSessionId && af.UserId == userId)
                .OrderByDescending(af => af.CreatedAt)
                .ToListAsync();
        }

        public async Task<AgentFile?> GetFileByIdAsync(int fileId)
        {
            return await _dbContext.AgentFiles
                .FirstOrDefaultAsync(af => af.Id == fileId);
        }

        public async Task<List<AgentFile>> GetFilesByUserAsync(Guid userId)
        {
            return await _dbContext.AgentFiles
                .Where(af => af.UserId == userId)
                .OrderByDescending(af => af.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateFileAsync(AgentFile agentFile)
        {
            _dbContext.AgentFiles.Update(agentFile);
            await _dbContext.SaveChangesAsync();
        }
    }
}
