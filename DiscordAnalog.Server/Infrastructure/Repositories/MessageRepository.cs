using DiscordAnalog.Server.Infrastructure.Data;
using DiscordAnalogModelsClassLibrary.Core.Entities;
using DiscordAnalogModelsClassLibrary.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория сообщений 
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор MessageRepository
        /// </summary>
        /// <param name="context"></param>
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение сообщения по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Message?> GetByIdAsync(int id) =>
            await _context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => int.Parse(m.Id) == id);

        /// <summary>
        /// Получение списка сообщений комнаты
        /// </summary>
        /// <param name="roomId">id комнаты</param>
        /// <param name="count">количество</param>
        /// <returns></returns>
        public async Task<IEnumerable<Message>> GetByRoomAsync(int roomId, int count) =>
            await _context.Messages
                .Where(m => int.Parse(m.ChatRoomId) == roomId)
                .Include(m => m.User)
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .ToListAsync();

        /// <summary>
        /// Добавление сообщения 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }
}
