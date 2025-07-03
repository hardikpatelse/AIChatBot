using AIChatBot.API.Models;
using AIChatBot.API.Models.Tools_Structure;

namespace AIChatBot.API.Interfaces
{
    public interface IChatModelService
    {
        Task<string> SendMessageAsync(string model, string message);
        Task<List<FunctionCallResult>> ChatWithFunctionSupportAsync(string model, string userMessage, List<ToolDefinition> functions);
    }
}
