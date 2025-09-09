using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class UserDocument
    {
        [FirestoreProperty("id")]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty("email")]
        public string Email { get; set; } = string.Empty;

        [FirestoreProperty("name")]
        public string Name { get; set; } = string.Empty;

        // Helper method to convert from entity
        public static UserDocument FromEntity(AIChatBot.API.Models.Entities.User user)
        {
            return new UserDocument
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Name = user.Name
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.User ToEntity()
        {
            return new AIChatBot.API.Models.Entities.User
            {
                Id = Guid.Parse(Id),
                Email = Email,
                Name = Name
            };
        }
    }
}