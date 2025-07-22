using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.DataContext
{
    public class UserDataContext : IUserDataContext
    {
        private readonly ChatBotDbContext _dbContext;
        public UserDataContext(ChatBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbContext.Users
                .Include(u => u.ChatSessions)
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task RegisterUser(Guid userId, string name, string email)
        {
            await _dbContext.Users.AddAsync(new User
            {
                Id = userId,
                Name = name,
                Email = email,
            });
            await _dbContext.SaveChangesAsync();
        }


    }
}
