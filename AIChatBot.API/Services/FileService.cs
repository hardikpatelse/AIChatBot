using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Services
{
    public class FileService : IFileService
    {
        private readonly IAgentFileDataContext _agentFileDataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseFilePath = Path.Combine("AgentFiles");

        public FileService(IAgentFileDataContext agentFileDataContext, IHttpContextAccessor httpContextAccessor)
        {
            _agentFileDataContext = agentFileDataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateFileAsync(string fileName, string content, Guid userId, int chatSessionId)
        {
            try
            {
                // Create directory structure: AgentFiles/[USER_ID]/[SESSION_ID]
                var userDir = Path.Combine(_baseFilePath, userId.ToString());
                var sessionDir = Path.Combine(userDir, chatSessionId.ToString());

                // Ensure directories exist
                Directory.CreateDirectory(sessionDir);

                // Create full file path
                // Validate and sanitize the fileName to prevent path traversal
                fileName = SanitizeFileName(fileName);
                var filePath = Path.Combine(sessionDir, fileName);

                // Ensure the resolved filePath is within the intended directory
                if (!filePath.StartsWith(sessionDir))
                {
                    throw new UnauthorizedAccessException("Invalid file name or path traversal attempt detected.");
                }
                // Write file content
                await File.WriteAllTextAsync(filePath, content);

                // Get file size
                var fileInfo = new FileInfo(filePath);
                var fileSize = fileInfo.Length;

                // Create download URL
                var downloadUrl = $"/api/files/download/{fileName}"; // This will be updated with file ID after saving

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

                // Get the current base URL from the request context if available
                var baseUrl = string.Empty;

                if (_httpContextAccessor.HttpContext != null)
                {
                    var request = _httpContextAccessor.HttpContext.Request;
                    baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
                }

                return $"✅ File '{fileName}' created successfully. Download the file from <a href='{baseUrl}/api/Files/download/{agentFile.Id}'>here</a>.";
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

        public async Task<AgentFile?> GetFileAsync(int fileId)
        {
            return await _agentFileDataContext.GetFileByIdAsync(fileId);
        }

        public async Task<Stream?> GetFileStreamAsync(int fileId)
        {
            var agentFile = await _agentFileDataContext.GetFileByIdAsync(fileId);
            if (agentFile == null || !File.Exists(agentFile.FilePath))
            {
                return null;
            }

            using (var fileStream = new FileStream(agentFile.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset the position to the beginning of the stream
                return memoryStream;
            }
        }
    }
}
