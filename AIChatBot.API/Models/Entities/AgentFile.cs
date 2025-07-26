namespace AIChatBot.API.Models.Entities
{
    public class AgentFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public Guid UserId { get; set; }
        public int ChatSessionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User User { get; set; } = null!;
        public ChatSession ChatSession { get; set; } = null!;
    }
}