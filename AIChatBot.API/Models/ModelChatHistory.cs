using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Models
{
    public class ModelChatHistory
    {
        public int ModelId { get; set; }
        public Guid ChatSessionIdentity { get; set; }
        public List<ChatMessage> History { get; set; }
    }
}