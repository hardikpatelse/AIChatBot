namespace AIChatBot.API.Models.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatSessionId { get; set; }
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime TimeStamp { get; set; }
    }
}
