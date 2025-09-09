using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class AIModelChatModeDocument
    {
        [FirestoreProperty("aiModelId")]
        public int AIModelId { get; set; }

        [FirestoreProperty("chatModeId")]
        public int ChatModeId { get; set; }

        // Helper method to convert from entity
        public static AIModelChatModeDocument FromEntity(AIChatBot.API.Models.Entities.AIModelChatMode modelChatMode)
        {
            return new AIModelChatModeDocument
            {
                AIModelId = modelChatMode.AIModelId,
                ChatModeId = modelChatMode.ChatModeId
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.AIModelChatMode ToEntity()
        {
            return new AIChatBot.API.Models.Entities.AIModelChatMode
            {
                AIModelId = AIModelId,
                ChatModeId = ChatModeId
            };
        }
    }
}