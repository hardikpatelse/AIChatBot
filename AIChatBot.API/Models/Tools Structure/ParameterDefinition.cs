using System.Text.Json.Serialization;

namespace AIChatBot.API.Models.Tools_Structure
{
    public class ParameterDefinition
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "object";
        [JsonPropertyName("properties")]
        public Dictionary<string, PropertyDefinition> Properties { get; set; }
        [JsonPropertyName("required")]
        public List<string> Required { get; set; } = new();
    }
}
