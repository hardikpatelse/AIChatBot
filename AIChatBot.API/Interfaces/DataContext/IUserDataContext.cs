using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.DataContext
{
    public interface IUserDataContext
    {
        Task<User> GetUserByEmail(string email);
        Task RegisterUser(Guid userId, string name, string email);
    }
}
