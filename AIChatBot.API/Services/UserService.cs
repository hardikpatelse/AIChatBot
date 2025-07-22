using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataContext _userDataContext;
        public UserService(IUserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userDataContext.GetUserByEmail(email);
        }

        public async Task<User> RegisterUser(string name, string email)
        {
            var userId = Guid.NewGuid();
            await _userDataContext.RegisterUser(userId, name, email);
            var user = new User
            {
                Name = name,
                Email = email,
                Id = userId
            };
            return user;
        }
    }
}
