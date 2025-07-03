namespace AIChatBot.API.Models
{
    public class FunctionCallResult
    {
        public string? FunctionName { get; set; }
        public string? ArgumentsJson { get; set; }
        public string? TextResponse { get; set; }
    }

}
