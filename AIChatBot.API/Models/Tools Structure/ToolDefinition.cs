using System.Text.Json.Serialization;

namespace AIChatBot.API.Models.Tools_Structure
{
    public class ToolDefinition
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("function")]
        public FunctionDefinition Function { get; set; }
    }
}
