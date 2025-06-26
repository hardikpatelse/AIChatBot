namespace AIChatBot.API.Models
{
    public class ChatRequest
    {
        public string Model { get; set; }
        public string Message { get; set; }
        public string AIMode { get; set; } // If true, use AgentService; otherwise, use factory
    }
}
