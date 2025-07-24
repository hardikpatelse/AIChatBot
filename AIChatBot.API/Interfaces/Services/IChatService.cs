using AIChatBot.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.API.Interfaces.Services
{
    public interface IChatService
    {
        IActionResult GetHistory(Guid userId, Guid chatSessionIdentity);
        Task<IActionResult> PostChat(ChatRequest request);
    }
}
