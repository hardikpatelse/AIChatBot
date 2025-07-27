using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.DataContext
{
    public interface IAgentFileDataContext
    {
        Task<AgentFile> CreateFileAsync(string fileName, string filePath, string downloadUrl, long fileSize, Guid userId, int chatSessionId);
        Task<List<AgentFile>> GetFilesBySessionAsync(int chatSessionId, Guid userId);
        Task<AgentFile?> GetFileByIdAsync(int fileId);
        Task<List<AgentFile>> GetFilesByUserAsync(Guid userId);
        Task UpdateFileAsync(AgentFile agentFile);
    }
}
