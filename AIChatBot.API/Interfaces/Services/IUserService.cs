using AIChatBot.API.Models.Entities;

namespace AIChatBot.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<User> RegisterUser(string name, string email);
    }
}
