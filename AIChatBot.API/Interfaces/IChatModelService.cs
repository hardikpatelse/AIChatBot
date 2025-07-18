﻿using AIChatBot.API.Models;

namespace AIChatBot.API.Interfaces
{
    public interface IChatModelService
    {
        Task<string> SendMessageAsync(string model, string message);
        Task<List<FunctionCallResult>> ChatWithFunctionSupportAsync(string model, List<Dictionary<string, string>> userMessage);
    }
}
