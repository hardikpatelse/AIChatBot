﻿using AIChatBot.API.Models;

namespace AIChatBot.API.Interfaces.Services
{
    public interface IChatModelService
    {
        Task<string> SendMessageAsync(string model, string message, string connectionId);
        Task<List<FunctionCallResult>> ChatWithFunctionSupportAsync(string model, List<Dictionary<string, string>> userMessage, string connectionId);
    }
}
