namespace AIChatBot.API.Models.Entities
{
    public class ChatSession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid UniqueIdentity { get; set; }
        public Guid UserId { get; set; }
        public int ModelId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
