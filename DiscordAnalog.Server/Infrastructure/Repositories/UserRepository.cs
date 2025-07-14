using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    /// <summary> Реализация репозитория пользователей </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext contest) => _context = contest;

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
