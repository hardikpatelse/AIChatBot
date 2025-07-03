using System.Text.Json.Serialization;

namespace AIChatBot.API.Models.Tools_Structure
{
    public class FunctionDefinition
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
        [JsonPropertyName("parameters")]
        public ParameterDefinition Parameters { get; set; } = new();
        
    }
}
