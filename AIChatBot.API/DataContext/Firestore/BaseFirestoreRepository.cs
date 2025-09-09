using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using AIChatBot.API.Models.Base;
using Microsoft.Extensions.Logging;

namespace AIChatBot.API.DataContext.Firestore
{
    public abstract class BaseFirestoreRepository
    {
        protected readonly FirestoreDb _firestoreDb;
        protected readonly ILogger _logger;
        protected readonly FirestoreSettings _settings;

        protected BaseFirestoreRepository(FirestoreDb firestoreDb, ILogger logger, IOptions<FirestoreSettings> settings)
        {
            _firestoreDb = firestoreDb;
            _logger = logger;
            _settings = settings.Value;
        }

        protected async Task<T?> GetDocumentAsync<T>(string collectionName, string documentId) where T : class
        {
            try
            {
                var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
                var snapshot = await docRef.GetSnapshotAsync();
                
                if (!snapshot.Exists)
                {
                    _logger.LogWarning("Document {DocumentId} not found in collection {CollectionName}", documentId, collectionName);
                    return null;
                }

                return snapshot.ConvertTo<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting document {DocumentId} from collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }

        protected async Task<List<T>> GetCollectionAsync<T>(string collectionName) where T : class
        {
            try
            {
                var collection = _firestoreDb.Collection(collectionName);
                var snapshot = await collection.GetSnapshotAsync();
                
                return snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting collection {CollectionName}", collectionName);
                throw;
            }
        }

        protected async Task<List<T>> QueryCollectionAsync<T>(string collectionName, Query query) where T : class
        {
            try
            {
                var snapshot = await query.GetSnapshotAsync();
                return snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying collection {CollectionName}", collectionName);
                throw;
            }
        }

        protected async Task<string> AddDocumentAsync<T>(string collectionName, T document) where T : class
        {
            try
            {
                var collection = _firestoreDb.Collection(collectionName);
                var docRef = await collection.AddAsync(document);
                
                _logger.LogInformation("Document added to collection {CollectionName} with ID {DocumentId}", collectionName, docRef.Id);
                return docRef.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding document to collection {CollectionName}", collectionName);
                throw;
            }
        }

        protected async Task SetDocumentAsync<T>(string collectionName, string documentId, T document) where T : class
        {
            try
            {
                var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
                await docRef.SetAsync(document);
                
                _logger.LogInformation("Document {DocumentId} set in collection {CollectionName}", documentId, collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting document {DocumentId} in collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }

        protected async Task UpdateDocumentAsync(string collectionName, string documentId, Dictionary<string, object> updates)
        {
            try
            {
                var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
                await docRef.UpdateAsync(updates);
                
                _logger.LogInformation("Document {DocumentId} updated in collection {CollectionName}", documentId, collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document {DocumentId} in collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }

        protected async Task DeleteDocumentAsync(string collectionName, string documentId)
        {
            try
            {
                var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
                await docRef.DeleteAsync();
                
                _logger.LogInformation("Document {DocumentId} deleted from collection {CollectionName}", documentId, collectionName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document {DocumentId} from collection {CollectionName}", documentId, collectionName);
                throw;
            }
        }
    }
}