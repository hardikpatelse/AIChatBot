namespace AIChatBot.API.Models.ToolResponse
{
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
        public List<ToolCall> tool_calls { get; set; }
    }
}
