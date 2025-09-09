using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class AgentFileDocument
    {
        [FirestoreProperty("id")]
        public int Id { get; set; }

        [FirestoreProperty("fileName")]
        public string FileName { get; set; } = string.Empty;

        [FirestoreProperty("filePath")]
        public string FilePath { get; set; } = string.Empty;

        [FirestoreProperty("downloadUrl")]
        public string DownloadUrl { get; set; } = string.Empty;

        [FirestoreProperty("fileSize")]
        public long FileSize { get; set; }

        [FirestoreProperty("userId")]
        public string UserId { get; set; } = string.Empty;

        [FirestoreProperty("chatSessionId")]
        public int ChatSessionId { get; set; }

        [FirestoreProperty("createdAt")]
        public Timestamp CreatedAt { get; set; }

        // Helper method to convert from entity
        public static AgentFileDocument FromEntity(AIChatBot.API.Models.Entities.AgentFile file)
        {
            return new AgentFileDocument
            {
                Id = file.Id,
                FileName = file.FileName,
                FilePath = file.FilePath,
                DownloadUrl = file.DownloadUrl,
                FileSize = file.FileSize,
                UserId = file.UserId.ToString(),
                ChatSessionId = file.ChatSessionId,
                CreatedAt = Timestamp.FromDateTime(file.CreatedAt.ToUniversalTime())
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.AgentFile ToEntity()
        {
            return new AIChatBot.API.Models.Entities.AgentFile
            {
                Id = Id,
                FileName = FileName,
                FilePath = FilePath,
                DownloadUrl = DownloadUrl,
                FileSize = FileSize,
                UserId = Guid.Parse(UserId),
                ChatSessionId = ChatSessionId,
                CreatedAt = CreatedAt.ToDateTime()
            };
        }
    }
}