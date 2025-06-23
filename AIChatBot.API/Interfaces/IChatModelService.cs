namespace AIChatBot.API.Interfaces
{
    public interface IChatModelService
    {
        Task<string> SendMessageAsync(string model, string message);
    }
}
