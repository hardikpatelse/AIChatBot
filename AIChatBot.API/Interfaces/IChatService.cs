using AIChatBot.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Interfaces
{
    public interface IChatService
    {
        IActionResult GetHistory(string modelId);
        Task<IActionResult> PostChat(ChatRequest request);
    }
}
