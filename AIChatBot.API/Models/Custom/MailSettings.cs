namespace AIChatBot.API.Models.Custom
{
    public class MailSettings
    {
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string APIToken { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
