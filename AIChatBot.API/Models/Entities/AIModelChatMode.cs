namespace AIChatBot.API.Models.Entities
{
    public class AIModelChatMode
    {
        public int AIModelId { get; set; }
        public AIModel AIModel { get; set; }

        public int ChatModeId { get; set; }
        public ChatMode ChatMode { get; set; }
    }
}
