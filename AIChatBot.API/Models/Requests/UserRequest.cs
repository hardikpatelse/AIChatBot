using System.Text.Json.Serialization;

namespace AIChatBot.API.Models.Requests
{
    public class UserRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}
