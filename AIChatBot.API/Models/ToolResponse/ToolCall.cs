namespace AIChatBot.API.Models.ToolResponse
{
    public class ToolCall
    {
        public int index { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public Function function { get; set; }
    }
}
