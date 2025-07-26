using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Services
{
    public class FileService : IFileService
    {
        private readonly IAgentFileDataContext _agentFileDataContext;

        public FileService(IAgentFileDataContext agentFileDataContext)
        {
            _agentFileDataContext = agentFileDataContext;
        }

        public async Task<string> CreateFileAsync(string fileName, string content, Guid userId, int chatSessionId)
        {
            try
            {
                // Create directory structure: AgentFiles/[USER_ID]/[SESSION_ID]
                var userDir = Path.Combine("AgentFiles", userId.ToString());
                var sessionDir = Path.Combine(userDir, chatSessionId.ToString());
                
                // Ensure directories exist
                Directory.CreateDirectory(sessionDir);
                
                // Create full file path
                var filePath = Path.Combine(sessionDir, fileName);
                
                // Write file content
                await File.WriteAllTextAsync(filePath, content);
                
                // Get file size
                var fileInfo = new FileInfo(filePath);
                var fileSize = fileInfo.Length;
                
                // Create temporary download URL (will be updated after saving)
                var downloadUrl = "/api/files/download/0"; 
                
                // Save file metadata to database
                var agentFile = await _agentFileDataContext.CreateFileAsync(
                    fileName, 
                    filePath, 
                    downloadUrl, 
                    fileSize, 
                    userId, 
                    chatSessionId);
                
                // Update download URL with actual file ID
                agentFile.DownloadUrl = $"/api/files/download/{agentFile.Id}";
                await _agentFileDataContext.UpdateFileAsync(agentFile);
                
                return $"✅ File '{fileName}' created successfully. File ID: {agentFile.Id}";
            }
            catch (Exception ex)
            {
                return $"❌ Failed to create file '{fileName}': {ex.Message}";
            }
        }

        public async Task<List<AgentFile>> GetSessionFilesAsync(int chatSessionId, Guid userId)
        {
            return await _agentFileDataContext.GetFilesBySessionAsync(chatSessionId, userId);
        }

        public async Task<AgentFile?> GetFileAsync(int fileId, Guid userId)
        {
            return await _agentFileDataContext.GetFileByIdAsync(fileId, userId);
        }

        public async Task<Stream?> GetFileStreamAsync(int fileId, Guid userId)
        {
            var agentFile = await _agentFileDataContext.GetFileByIdAsync(fileId, userId);
            if (agentFile == null || !File.Exists(agentFile.FilePath))
            {
                return null;
            }
            
            return new FileStream(agentFile.FilePath, FileMode.Open, FileAccess.Read);
        }
    }
}