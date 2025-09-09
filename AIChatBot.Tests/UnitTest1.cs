using AIChatBot.API.DataContext.Firestore;
using AIChatBot.API.Models.Base;
using AIChatBot.API.Models.Entities;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AIChatBot.Tests
{
    public class FirestoreDataContextTests
    {
        private readonly FirestoreSettings _firestoreSettings;

        public FirestoreDataContextTests()
        {
            _firestoreSettings = new FirestoreSettings
            {
                ProjectId = "test-project",
                DatabaseId = "(default)",
                UseEmulator = true,
                EmulatorHost = "localhost:8080"
            };
        }

        [Fact]
        public void FirestoreSettings_HasCorrectDefaults()
        {
            // Arrange
            var settings = new FirestoreSettings();

            // Assert
            Assert.Equal("(default)", settings.DatabaseId);
            Assert.Equal(30, settings.TimeoutSeconds);
            Assert.False(settings.UseEmulator);
            Assert.Equal("localhost:8080", settings.EmulatorHost);
        }

        [Fact]
        public void FirestoreSettings_CanBeConfigured()
        {
            // Arrange & Act
            var settings = new FirestoreSettings
            {
                ProjectId = "custom-project",
                DatabaseId = "custom-db",
                UseEmulator = true,
                EmulatorHost = "localhost:9090",
                TimeoutSeconds = 60
            };

            // Assert
            Assert.Equal("custom-project", settings.ProjectId);
            Assert.Equal("custom-db", settings.DatabaseId);
            Assert.True(settings.UseEmulator);
            Assert.Equal("localhost:9090", settings.EmulatorHost);
            Assert.Equal(60, settings.TimeoutSeconds);
        }

        [Fact]
        public void UserDocument_CanConvertFromEntity()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Name = "Test User"
            };

            // Act
            var userDoc = AIChatBot.API.Models.Firestore.UserDocument.FromEntity(user);

            // Assert
            Assert.Equal(user.Id.ToString(), userDoc.Id);
            Assert.Equal(user.Email, userDoc.Email);
            Assert.Equal(user.Name, userDoc.Name);
        }

        [Fact]
        public void UserDocument_CanConvertToEntity()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDoc = new AIChatBot.API.Models.Firestore.UserDocument
            {
                Id = userId.ToString(),
                Email = "test@example.com",
                Name = "Test User"
            };

            // Act
            var user = userDoc.ToEntity();

            // Assert
            Assert.Equal(userId, user.Id);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("Test User", user.Name);
        }

        [Fact]
        public void ChatSessionDocument_CanConvertFromEntity()
        {
            // Arrange
            var session = new ChatSession
            {
                Id = 1,
                Name = "Test Session",
                UniqueIdentity = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var sessionDoc = AIChatBot.API.Models.Firestore.ChatSessionDocument.FromEntity(session);

            // Assert
            Assert.Equal(session.Id, sessionDoc.Id);
            Assert.Equal(session.Name, sessionDoc.Name);
            Assert.Equal(session.UniqueIdentity.ToString(), sessionDoc.UniqueIdentity);
            Assert.Equal(session.UserId.ToString(), sessionDoc.UserId);
        }

        [Fact]
        public void ChatSessionDocument_CanConvertToEntity()
        {
            // Arrange
            var sessionId = 1;
            var uniqueIdentity = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var sessionDoc = new AIChatBot.API.Models.Firestore.ChatSessionDocument
            {
                Id = sessionId,
                Name = "Test Session",
                UniqueIdentity = uniqueIdentity.ToString(),
                UserId = userId.ToString(),
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            // Act
            var session = sessionDoc.ToEntity();

            // Assert
            Assert.Equal(sessionId, session.Id);
            Assert.Equal("Test Session", session.Name);
            Assert.Equal(uniqueIdentity, session.UniqueIdentity);
            Assert.Equal(userId, session.UserId);
        }

        [Fact]
        public void AgentFileDocument_CanConvertFromEntity()
        {
            // Arrange
            var file = new AgentFile
            {
                Id = 1,
                FileName = "test.txt",
                FilePath = "/path/to/test.txt",
                DownloadUrl = "https://example.com/test.txt",
                FileSize = 1024,
                UserId = Guid.NewGuid(),
                ChatSessionId = 1,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var fileDoc = AIChatBot.API.Models.Firestore.AgentFileDocument.FromEntity(file);

            // Assert
            Assert.Equal(file.Id, fileDoc.Id);
            Assert.Equal(file.FileName, fileDoc.FileName);
            Assert.Equal(file.FilePath, fileDoc.FilePath);
            Assert.Equal(file.DownloadUrl, fileDoc.DownloadUrl);
            Assert.Equal(file.FileSize, fileDoc.FileSize);
            Assert.Equal(file.UserId.ToString(), fileDoc.UserId);
            Assert.Equal(file.ChatSessionId, fileDoc.ChatSessionId);
        }

        [Fact]
        public void AgentFileDocument_CanConvertToEntity()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var fileDoc = new AIChatBot.API.Models.Firestore.AgentFileDocument
            {
                Id = 1,
                FileName = "test.txt",
                FilePath = "/path/to/test.txt",
                DownloadUrl = "https://example.com/test.txt",
                FileSize = 1024,
                UserId = userId.ToString(),
                ChatSessionId = 1,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            // Act
            var file = fileDoc.ToEntity();

            // Assert
            Assert.Equal(1, file.Id);
            Assert.Equal("test.txt", file.FileName);
            Assert.Equal("/path/to/test.txt", file.FilePath);
            Assert.Equal("https://example.com/test.txt", file.DownloadUrl);
            Assert.Equal(1024, file.FileSize);
            Assert.Equal(userId, file.UserId);
            Assert.Equal(1, file.ChatSessionId);
        }
    }
}