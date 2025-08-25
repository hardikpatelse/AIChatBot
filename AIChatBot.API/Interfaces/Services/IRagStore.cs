namespace AIChatBot.API.Interfaces.Services
{
    public interface IRagStore
    {
        Task IndexAsync(string userId, string docId, string content);
        Task<List<string>> SearchAsync(string userId, string query, int topK = 3);
        Task<List<string>> GetDocumentIdsAsync(string userId);
        Task<bool> DeleteDocumentAsync(string userId, string docId);
    }
}