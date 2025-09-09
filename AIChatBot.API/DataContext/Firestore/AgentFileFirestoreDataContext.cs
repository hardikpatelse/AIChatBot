using AIChatBot.API.DataContext.Firestore;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Base;
using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Firestore;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AIChatBot.API.DataContext.Firestore
{
    public class AgentFileFirestoreDataContext : BaseFirestoreRepository, IAgentFileDataContext
    {
        private const string COLLECTION_NAME = "agentFiles";

        public AgentFileFirestoreDataContext(FirestoreDb firestoreDb, ILogger<AgentFileFirestoreDataContext> logger, IOptions<FirestoreSettings> settings)
            : base(firestoreDb, logger, settings)
        {
        }

        public async Task<AgentFile> CreateFileAsync(string fileName, string filePath, string downloadUrl, long fileSize, Guid userId, int chatSessionId)
        {
            try
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

                var fileDoc = AgentFileDocument.FromEntity(agentFile);
                
                // Add document and get the auto-generated ID
                var docRef = await _firestoreDb.Collection(COLLECTION_NAME).AddAsync(fileDoc);
                
                // Update the document with the auto-generated ID as the file ID
                agentFile.Id = int.Parse(docRef.Id);
                fileDoc.Id = agentFile.Id;
                await docRef.SetAsync(fileDoc);

                _logger.LogInformation("Created agent file {FileId} for session {SessionId}", agentFile.Id, chatSessionId);
                return agentFile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating agent file for session {SessionId}", chatSessionId);
                throw;
            }
        }

        public async Task<List<AgentFile>> GetFilesBySessionAsync(int chatSessionId, Guid userId)
        {
            try
            {
                var query = _firestoreDb.Collection(COLLECTION_NAME)
                    .WhereEqualTo("chatSessionId", chatSessionId)
                    .WhereEqualTo("userId", userId.ToString())
                    .OrderByDescending("createdAt");

                var snapshot = await query.GetSnapshotAsync();
                var files = snapshot.Documents
                    .Select(doc => doc.ConvertTo<AgentFileDocument>().ToEntity())
                    .ToList();

                _logger.LogInformation("Retrieved {Count} files for session {SessionId}", files.Count, chatSessionId);
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting files for session {SessionId}", chatSessionId);
                throw;
            }
        }

        public async Task<AgentFile?> GetFileByIdAsync(int fileId)
        {
            try
            {
                var query = _firestoreDb.Collection(COLLECTION_NAME).WhereEqualTo("id", fileId);
                var snapshot = await query.GetSnapshotAsync();

                if (!snapshot.Documents.Any())
                {
                    _logger.LogWarning("Agent file with ID {FileId} not found", fileId);
                    return null;
                }

                var fileDoc = snapshot.Documents.First().ConvertTo<AgentFileDocument>();
                return fileDoc.ToEntity();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file by ID {FileId}", fileId);
                throw;
            }
        }

        public async Task<List<AgentFile>> GetFilesByUserAsync(Guid userId)
        {
            try
            {
                var query = _firestoreDb.Collection(COLLECTION_NAME)
                    .WhereEqualTo("userId", userId.ToString())
                    .OrderByDescending("createdAt");

                var snapshot = await query.GetSnapshotAsync();
                var files = snapshot.Documents
                    .Select(doc => doc.ConvertTo<AgentFileDocument>().ToEntity())
                    .ToList();

                _logger.LogInformation("Retrieved {Count} files for user {UserId}", files.Count, userId);
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting files for user {UserId}", userId);
                throw;
            }
        }

        public async Task UpdateFileAsync(AgentFile agentFile)
        {
            try
            {
                var fileDoc = AgentFileDocument.FromEntity(agentFile);
                var query = _firestoreDb.Collection(COLLECTION_NAME).WhereEqualTo("id", agentFile.Id);
                var snapshot = await query.GetSnapshotAsync();

                if (!snapshot.Documents.Any())
                {
                    _logger.LogWarning("Agent file with ID {FileId} not found for update", agentFile.Id);
                    return;
                }

                var doc = snapshot.Documents.First();
                await doc.Reference.SetAsync(fileDoc);
                
                _logger.LogInformation("Updated agent file {FileId}", agentFile.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating agent file {FileId}", agentFile.Id);
                throw;
            }
        }
    }
}