using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class ChatSessionDocument
    {
        [FirestoreProperty("id")]
        public int Id { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty("uniqueIdentity")]
        public string UniqueIdentity { get; set; } = string.Empty;

        [FirestoreProperty("userId")]
        public string UserId { get; set; } = string.Empty;

        [FirestoreProperty("createdAt")]
        public Timestamp CreatedAt { get; set; }

        // Helper method to convert from entity
        public static ChatSessionDocument FromEntity(AIChatBot.API.Models.Entities.ChatSession session)
        {
            return new ChatSessionDocument
            {
                Id = session.Id,
                Name = session.Name,
                UniqueIdentity = session.UniqueIdentity.ToString(),
                UserId = session.UserId.ToString(),
                CreatedAt = Timestamp.FromDateTime(session.CreatedAt.ToUniversalTime())
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.ChatSession ToEntity()
        {
            return new AIChatBot.API.Models.Entities.ChatSession
            {
                Id = Id,
                Name = Name,
                UniqueIdentity = Guid.Parse(UniqueIdentity),
                UserId = Guid.Parse(UserId),
                CreatedAt = CreatedAt.ToDateTime()
            };
        }
    }
}