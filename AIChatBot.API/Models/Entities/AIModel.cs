namespace AIChatBot.API.Models.Entities
{
    public class AIModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string ReferenceLink { get; set; }
        public string ReferralSource { get; set; }
        public ICollection<AIModelChatMode> ChatModes { get; set; } = new List<AIModelChatMode>();
    }
}