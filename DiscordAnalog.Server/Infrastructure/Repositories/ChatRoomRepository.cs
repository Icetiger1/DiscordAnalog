using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Core.Entities;
using DiscordAnalog.Server.Core.Interfaces;
using DiscordAnalog.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly AppDbContext _context;

        public ChatRoomRepository(AppDbContext context) => _context = context;

        public async Task<ChatRoom?> GetByIdAsync(int roomId) =>
            await _context.ChatRooms.FindAsync(roomId);

        public async Task<IEnumerable<Message>> GetMessagesAsync(int roomId, int count) =>
            await _context.Messages
                .Where(m => m.ChatRoomId == roomId)
                .Include(m => m.User)
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .ToListAsync();

        public async Task AddMessageAsync(int roomId, Message message)
        {
            var room = await GetByIdAsync(roomId);
            if (room == null) throw new ArgumentException("Room not found");

            room.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
