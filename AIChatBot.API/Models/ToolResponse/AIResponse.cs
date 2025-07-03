using System.Text.Json.Serialization;

namespace AIChatBot.API.Models.ToolResponse
{
    public class AIResponse
    {
        public List<Choice> choices { get; set; }
    }
}
