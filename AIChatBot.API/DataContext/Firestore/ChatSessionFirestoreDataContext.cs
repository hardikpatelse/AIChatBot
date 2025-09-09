using AIChatBot.API.DataContext.Firestore;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Base;
using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Firestore;
using AIChatBot.API.Models.Requests;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AIChatBot.API.DataContext.Firestore
{
    public class ChatSessionFirestoreDataContext : BaseFirestoreRepository, IChatSessionDataContext
    {
        private const string COLLECTION_NAME = "chatSessions";

        public ChatSessionFirestoreDataContext(FirestoreDb firestoreDb, ILogger<ChatSessionFirestoreDataContext> logger, IOptions<FirestoreSettings> settings)
            : base(firestoreDb, logger, settings)
        {
        }

        public async Task<List<ChatSession>> GetSessionsWithoutMessages(Guid userId)
        {
            try
            {
                var query = _firestoreDb.Collection(COLLECTION_NAME).WhereEqualTo("userId", userId.ToString());
                var snapshot = await query.GetSnapshotAsync();

                var sessions = snapshot.Documents
                    .Select(doc => doc.ConvertTo<ChatSessionDocument>().ToEntity())
                    .ToList();

                _logger.LogInformation("Retrieved {Count} sessions for user {UserId}", sessions.Count, userId);
                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sessions for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> RenameChatSessionAsync(ChatSessionRequest request)
        {
            try
            {
                var query = _firestoreDb.Collection(COLLECTION_NAME).WhereEqualTo("id", request.Id);
                var snapshot = await query.GetSnapshotAsync();

                if (!snapshot.Documents.Any())
                {
                    _logger.LogWarning("Chat session with ID {SessionId} not found", request.Id);
                    return false;
                }

                var doc = snapshot.Documents.First();
                var updates = new Dictionary<string, object>
                {
                    { "name", request.Name }
                };

                await doc.Reference.UpdateAsync(updates);
                _logger.LogInformation("Chat session {SessionId} renamed to {NewName}", request.Id, request.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renaming chat session {SessionId}", request.Id);
                throw;
            }
        }

        public async Task<ChatSession> CreateSessionAsync(ChatSessionRequest request)
        {
            try
            {
                var session = new ChatSession
                {
                    Name = request.Name,
                    UserId = request.UserId!.Value,
                    UniqueIdentity = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Messages = new List<ChatMessage>()
                };

                var sessionDoc = ChatSessionDocument.FromEntity(session);
                
                // Add document and get the auto-generated ID
                var docRef = await _firestoreDb.Collection(COLLECTION_NAME).AddAsync(sessionDoc);
                
                // Update the document with the auto-generated ID as the session ID
                session.Id = int.Parse(docRef.Id);
                sessionDoc.Id = session.Id;
                await docRef.SetAsync(sessionDoc);

                _logger.LogInformation("Created chat session {SessionId} for user {UserId}", session.Id, session.UserId);
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat session for user {UserId}", request.UserId);
                throw;
            }
        }
    }
}