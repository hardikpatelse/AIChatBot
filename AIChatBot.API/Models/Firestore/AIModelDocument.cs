using Google.Cloud.Firestore;

namespace AIChatBot.API.Models.Firestore
{
    [FirestoreData]
    public class AIModelDocument
    {
        [FirestoreProperty("id")]
        public int Id { get; set; }

        [FirestoreProperty("modelName")]
        public string ModelName { get; set; } = string.Empty;

        [FirestoreProperty("name")]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty("company")]
        public string Company { get; set; } = string.Empty;

        [FirestoreProperty("logoUrl")]
        public string LogoUrl { get; set; } = string.Empty;

        [FirestoreProperty("description")]
        public string Description { get; set; } = string.Empty;

        [FirestoreProperty("referenceLink")]
        public string ReferenceLink { get; set; } = string.Empty;

        [FirestoreProperty("referralSource")]
        public string ReferralSource { get; set; } = string.Empty;

        // Helper method to convert from entity
        public static AIModelDocument FromEntity(AIChatBot.API.Models.Entities.AIModel model)
        {
            return new AIModelDocument
            {
                Id = model.Id,
                ModelName = model.ModelName,
                Name = model.Name,
                Company = model.Company,
                LogoUrl = model.LogoUrl,
                Description = model.Description,
                ReferenceLink = model.ReferenceLink,
                ReferralSource = model.ReferralSource
            };
        }

        // Helper method to convert to entity
        public AIChatBot.API.Models.Entities.AIModel ToEntity()
        {
            return new AIChatBot.API.Models.Entities.AIModel
            {
                Id = Id,
                ModelName = ModelName,
                Name = Name,
                Company = Company,
                LogoUrl = LogoUrl,
                Description = Description,
                ReferenceLink = ReferenceLink,
                ReferralSource = ReferralSource
            };
        }
    }
}