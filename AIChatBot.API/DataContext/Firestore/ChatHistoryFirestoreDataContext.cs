using AIChatBot.API.DataContext.Firestore;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Base;
using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Firestore;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AIChatBot.API.DataContext.Firestore
{
    public class ChatHistoryFirestoreDataContext : BaseFirestoreRepository, IChatHistoryDataContext
    {
        private const string SESSIONS_COLLECTION = "chatSessions";
        private const string MESSAGES_COLLECTION = "chatMessages";

        public ChatHistoryFirestoreDataContext(FirestoreDb firestoreDb, ILogger<ChatHistoryFirestoreDataContext> logger, IOptions<FirestoreSettings> settings)
            : base(firestoreDb, logger, settings)
        {
        }

        public ChatSession? GetHistory(Guid userId, Guid chatSessionIdentity)
        {
            try
            {
                // Get the chat session first
                var sessionQuery = _firestoreDb.Collection(SESSIONS_COLLECTION)
                    .WhereEqualTo("userId", userId.ToString())
                    .WhereEqualTo("uniqueIdentity", chatSessionIdentity.ToString());

                var sessionSnapshot = sessionQuery.GetSnapshotAsync().Result;

                if (!sessionSnapshot.Documents.Any())
                {
                    _logger.LogWarning("Chat session with identity {SessionIdentity} not found for user {UserId}", chatSessionIdentity, userId);
                    return null;
                }

                var sessionDoc = sessionSnapshot.Documents.First().ConvertTo<ChatSessionDocument>();
                var session = sessionDoc.ToEntity();

                // Get messages for this session
                var messagesQuery = _firestoreDb.Collection(MESSAGES_COLLECTION)
                    .WhereEqualTo("chatSessionId", session.Id)
                    .OrderBy("timeStamp");

                var messagesSnapshot = messagesQuery.GetSnapshotAsync().Result;
                var messages = messagesSnapshot.Documents
                    .Select(doc => doc.ConvertTo<ChatMessageDocument>().ToEntity())
                    .ToList();

                session.Messages = messages;

                _logger.LogInformation("Retrieved chat history for session {SessionId} with {MessageCount} messages", session.Id, messages.Count);
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat history for session identity {SessionIdentity}", chatSessionIdentity);
                throw;
            }
        }

        public void SaveHistory(Guid userId, List<ChatMessage> messages)
        {
            try
            {
                // Save each message to Firestore
                var tasks = messages.Select(async message =>
                {
                    var messageDoc = ChatMessageDocument.FromEntity(message);
                    
                    // Check if message already exists
                    var query = _firestoreDb.Collection(MESSAGES_COLLECTION).WhereEqualTo("id", message.Id);
                    var snapshot = await query.GetSnapshotAsync();

                    if (snapshot.Documents.Any())
                    {
                        // Update existing message
                        var doc = snapshot.Documents.First();
                        await doc.Reference.SetAsync(messageDoc);
                    }
                    else
                    {
                        // Add new message
                        await _firestoreDb.Collection(MESSAGES_COLLECTION).AddAsync(messageDoc);
                    }
                });

                Task.WaitAll(tasks.ToArray());

                _logger.LogInformation("Saved {MessageCount} messages for user {UserId}", messages.Count, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving chat history for user {UserId}", userId);
                throw;
            }
        }
    }
}