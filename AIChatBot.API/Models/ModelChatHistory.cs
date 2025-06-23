using System;
using System.Collections.Generic;

namespace AIChatBot.API.Models
{
    public class ModelChatHistory
    {
        public string ModelId { get; set; }
        public List<ChatMessage> History { get; set; }
    }
}