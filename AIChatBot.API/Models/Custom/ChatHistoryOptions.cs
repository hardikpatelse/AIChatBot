namespace AIChatBot.API.Models.Custom
{
    public class ChatHistoryOptions
    {
        public int RetryCount { get; set; } = 3;
        public int RetryDelayMilliseconds { get; set; } = 200;
    }
}
