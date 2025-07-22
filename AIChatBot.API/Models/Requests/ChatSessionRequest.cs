namespace AIChatBot.API.Models.Requests
{
    public class ChatSessionRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? UserId { get; set; } // Optional for create
        public int? ModelId { get; set; } // Optional for create
    }
}
