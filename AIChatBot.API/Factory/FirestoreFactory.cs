using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using AIChatBot.API.Models.Base;

namespace AIChatBot.API.Factory
{
    public class FirestoreFactory
    {
        private readonly FirestoreSettings _settings;

        public FirestoreFactory(IOptions<FirestoreSettings> settings)
        {
            _settings = settings.Value;
        }

        public FirestoreDb CreateFirestoreDb()
        {
            if (_settings.UseEmulator)
            {
                Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", _settings.EmulatorHost);
            }

            FirestoreDbBuilder builder = new FirestoreDbBuilder
            {
                ProjectId = _settings.ProjectId,
                DatabaseId = _settings.DatabaseId
            };

            if (!string.IsNullOrEmpty(_settings.CredentialsPath) && !_settings.UseEmulator)
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _settings.CredentialsPath);
            }

            return builder.Build();
        }
    }
}