namespace AIChatBot.API.Models
{
    public class ChatMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime DateTime { get; set; }
    }
}
