using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> CreateFileAsync(string fileName, string content, Guid userId, int chatSessionId);
        Task<List<AgentFile>> GetSessionFilesAsync(int chatSessionId, Guid userId);
        Task<AgentFile?> GetFileAsync(int fileId, Guid userId);
        Task<Stream?> GetFileStreamAsync(int fileId, Guid userId);
    }
}