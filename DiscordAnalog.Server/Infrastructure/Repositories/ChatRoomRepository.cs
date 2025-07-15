using DiscordAnalog.Server.Infrastructure.Data;
using DiscordAnalogModelsClassLibrary.Core.Entities;
using DiscordAnalogModelsClassLibrary.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория канала 
    /// </summary>
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор ChatRoomRepository
        /// </summary>
        /// <param name="context"></param>
        public ChatRoomRepository(AppDbContext context) => _context = context;

        public async Task<ChatRoom?> GetByIdAsync(int roomId) =>
            await _context.ChatRooms.FindAsync(roomId);

        /// <summary>
        /// Получение списка сообщений в комнате
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Message>> GetMessagesAsync(int roomId, int count) =>
            await _context.Messages
                .Where(m => int.Parse(m.ChatRoomId) == roomId)
                .Include(m => m.User)
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .ToListAsync();

        /// <summary>
        /// Добавление сообщения
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task AddMessageAsync(int roomId, Message message)
        {
            var room = await GetByIdAsync(roomId);
            if (room == null) throw new ArgumentException("Room not found");

            room.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
