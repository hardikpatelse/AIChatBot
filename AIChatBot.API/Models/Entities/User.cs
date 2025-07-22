namespace AIChatBot.API.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; }
        public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
    }
}
