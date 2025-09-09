namespace AIChatBot.API.Models.Base
{
    public class FirestoreSettings
    {
        public string ProjectId { get; set; } = string.Empty;
        public string CredentialsPath { get; set; } = string.Empty;
        public string DatabaseId { get; set; } = "(default)";
        public int TimeoutSeconds { get; set; } = 30;
        public bool UseEmulator { get; set; } = false;
        public string EmulatorHost { get; set; } = "localhost:8080";
    }
}