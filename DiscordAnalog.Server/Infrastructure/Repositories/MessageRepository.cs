using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message?> GetByIdAsync(int id) =>
            await _context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<Message>> GetByRoomAsync(int roomId, int count) =>
            await _context.Messages
                .Where(m => m.ChatRoomId == roomId)
                .Include(m => m.User)
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .ToListAsync();

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }
}
