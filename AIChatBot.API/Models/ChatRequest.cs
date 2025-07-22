namespace AIChatBot.API.Models
{
    public class ChatRequest
    {
        public Guid UserId { get; set; }
        public Guid ChatSessionIdentity { get; set; }
        public int ModelId { get; set; }
        public string Message { get; set; }
        public string AIMode { get; set; } = string.Empty;
    }
}
