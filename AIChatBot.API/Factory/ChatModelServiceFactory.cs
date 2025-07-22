using AIChatBot.API.AIServices;
using AIChatBot.API.Interfaces.Services;

namespace AIChatBot.API.Factory
{
    public class ChatModelServiceFactory
    {
        private readonly IServiceProvider _provider;

        public ChatModelServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IChatModelService GetService(string modelName)
        {
            return modelName.ToLower() switch
            {
                "phi3" => _provider.GetRequiredService<OllamaChatService>(),
                "llama3" => _provider.GetRequiredService<OllamaChatService>(),
                "gemma:2b" => _provider.GetRequiredService<OllamaChatService>(),
                "mistral" => _provider.GetRequiredService<OllamaChatService>(),
                "deepseek/deepseek-chat-v3-0324:free" => _provider.GetRequiredService<OpenRouterChatService>(),
                "google/gemma-3-27b-it:free" => _provider.GetRequiredService<OpenRouterChatService>(),
                "google/gemini-2.0-flash-exp:free" => _provider.GetRequiredService<OpenRouterChatService>(),
                "openai/gpt-3.5-turbo-0613" => _provider.GetRequiredService<OpenRouterChatService>(),
                "google/gemini-2.0-flash-001" => _provider.GetRequiredService<OpenRouterChatService>(),
                _ => throw new NotSupportedException($"Model '{modelName}' is not supported.")
            };
        }
    }

}
