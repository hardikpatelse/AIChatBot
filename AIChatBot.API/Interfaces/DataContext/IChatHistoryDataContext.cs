﻿using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.DataContext
{
    public interface IChatHistoryDataContext
    {
        ChatSession? GetHistory(Guid userId, Guid chatSessionIdentity);
        void SaveHistory(Guid userId, List<ChatMessage> messages);
    }
}
