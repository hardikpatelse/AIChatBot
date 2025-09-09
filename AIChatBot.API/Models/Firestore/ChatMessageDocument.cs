using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class ChatMessageDocument
    {
        [FirestoreProperty("id")]
        public int Id { get; set; }

        [FirestoreProperty("chatSessionId")]
        public int ChatSessionId { get; set; }

        [FirestoreProperty("role")]
        public string Role { get; set; } = string.Empty;

        [FirestoreProperty("content")]
        public string Content { get; set; } = string.Empty;

        [FirestoreProperty("timeStamp")]
        public Timestamp TimeStamp { get; set; }

        // Helper method to convert from entity
        public static ChatMessageDocument FromEntity(AIChatBot.API.Models.Entities.ChatMessage message)
        {
            return new ChatMessageDocument
            {
                Id = message.Id,
                ChatSessionId = message.ChatSessionId,
                Role = message.Role,
                Content = message.Content,
                TimeStamp = Timestamp.FromDateTime(message.TimeStamp.ToUniversalTime())
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.ChatMessage ToEntity()
        {
            return new AIChatBot.API.Models.Entities.ChatMessage
            {
                Id = Id,
                ChatSessionId = ChatSessionId,
                Role = Role,
                Content = Content,
                TimeStamp = TimeStamp.ToDateTime()
            };
        }
    }
}