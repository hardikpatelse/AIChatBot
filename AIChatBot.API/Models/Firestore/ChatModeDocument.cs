using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class ChatModeDocument
    {
        [FirestoreProperty("id")]
        public int Id { get; set; }

        [FirestoreProperty("mode")]
        public string Mode { get; set; } = string.Empty;

        [FirestoreProperty("name")]
        public string Name { get; set; } = string.Empty;

        // Helper method to convert from entity
        public static ChatModeDocument FromEntity(AIChatBot.API.Models.Entities.ChatMode chatMode)
        {
            return new ChatModeDocument
            {
                Id = chatMode.Id,
                Mode = chatMode.Mode,
                Name = chatMode.Name
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.ChatMode ToEntity()
        {
            return new AIChatBot.API.Models.Entities.ChatMode
            {
                Id = Id,
                Mode = Mode,
                Name = Name
            };
        }
    }
}