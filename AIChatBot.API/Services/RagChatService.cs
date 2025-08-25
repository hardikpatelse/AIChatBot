using AIChatBot.API.Factory;
using AIChatBot.API.Interfaces.Services;

namespace AIChatBot.API.Services
{
    public class RagChatService
    {
        private readonly IRagStore _ragStore;
        private readonly ChatModelServiceFactory _modelFactory;
        private readonly IChatHistoryService _chatHistoryService;

        public RagChatService(
            IRagStore ragStore,
            ChatModelServiceFactory modelFactory,
            IChatHistoryService chatHistoryService)
        {
            _ragStore = ragStore;
            _modelFactory = modelFactory;
            _chatHistoryService = chatHistoryService;
        }

        public async Task<string> GenerateRagResponseAsync(
            string userId, 
            string query, 
            string modelName, 
            Guid chatSessionIdentity,
            string? connectionId = null)
        {
            // Retrieve relevant chunks
            var relevantChunks = await _ragStore.SearchAsync(userId, query, topK: 3);
            
            // Build contextual prompt
            var contextualPrompt = BuildContextualPrompt(query, relevantChunks);
            
            // Get AI service and generate response
            var aiService = _modelFactory.GetService(modelName);
            var response = await aiService.SendMessageAsync(modelName, contextualPrompt, connectionId ?? string.Empty);
            
            // Add source information if chunks were found
            if (relevantChunks.Any())
            {
                response += $"\n\n*Sources: {relevantChunks.Count} document(s) referenced*";
            }
            
            return response;
        }

        private string BuildContextualPrompt(string query, List<string> relevantChunks)
        {
            if (!relevantChunks.Any())
            {
                return $"User query: {query}\n\nNo relevant documents found. Please provide a general response based on your knowledge.";
            }

            var contextSection = string.Join("\n\n---\n\n", relevantChunks.Select((chunk, index) => 
                $"Document {index + 1}:\n{chunk}"));

            return $@"You are an AI assistant helping with queries based on provided context. Use the following context to answer the user's question accurately.

CONTEXT:
{contextSection}

USER QUESTION: {query}

Please provide a comprehensive answer based primarily on the provided context. If the context doesn't fully answer the question, you may supplement with general knowledge, but clearly indicate what comes from the provided documents versus general knowledge.";
        }
    }
}