using DiscordAnalog.Server.Infrastructure.Data;
using DiscordAnalogModelsClassLibrary.Core.Entities;
using DiscordAnalogModelsClassLibrary.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    /// <summary> 
    /// Реализация репозитория пользователей 
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор UserRepository
        /// </summary>
        /// <param name="contest">AppDbContext</param>
        public UserRepository(AppDbContext contest) => _context = contest;

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();

        /// <summary>
        /// Получение пользователя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetByIdAsync(string id) =>
            await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Получение пользователя по имени
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
