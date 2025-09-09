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
    public class UserFirestoreDataContext : BaseFirestoreRepository, IUserDataContext
    {
        private const string COLLECTION_NAME = "users";

        public UserFirestoreDataContext(FirestoreDb firestoreDb, ILogger<UserFirestoreDataContext> logger, IOptions<FirestoreSettings> settings)
            : base(firestoreDb, logger, settings)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var query = _firestoreDb.Collection(COLLECTION_NAME).WhereEqualTo("email", email);
                var snapshot = await query.GetSnapshotAsync();

                if (!snapshot.Documents.Any())
                {
                    _logger.LogWarning("User with email {Email} not found", email);
                    return null!;
                }

                var userDoc = snapshot.Documents.First().ConvertTo<UserDocument>();
                return userDoc.ToEntity();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                throw;
            }
        }

        public async Task RegisterUser(Guid userId, string name, string email)
        {
            try
            {
                var userDoc = new UserDocument
                {
                    Id = userId.ToString(),
                    Name = name,
                    Email = email
                };

                await SetDocumentAsync(COLLECTION_NAME, userId.ToString(), userDoc);
                _logger.LogInformation("User registered with ID {UserId} and email {Email}", userId, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user with ID {UserId} and email {Email}", userId, email);
                throw;
            }
        }
    }
}