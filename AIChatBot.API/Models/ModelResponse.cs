namespace AIChatBot.API.Models
{
    public class ModelResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string ReferenceLink { get; set; }
        public string ReferralSource { get; set; }
        public List<string> SupportedModes { get; set; }
    }
}