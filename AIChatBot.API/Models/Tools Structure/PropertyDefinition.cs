using System.Text.Json.Serialization;

namespace AIChatBot.API.Models.Tools_Structure
{
    public class PropertyDefinition                   
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "string"; //Array, Object, String, Number, Boolean
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
    }
}
