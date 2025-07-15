using DiscordAnalogModelsClassLibrary.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Data
{
    /// <summary>
    /// DbInitializer
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Наполнение БД первичными данными
        /// </summary>
        /// <param name="context"></param>
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any()) return;

            var users = new[]
            {
                new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Id = Guid.NewGuid().ToString() },
                new User { Username = "user1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("qwerty"), Id = Guid.NewGuid().ToString() }
            };

            var rooms = new[]
            {
                new ChatRoom { Name = "General", Id = Guid.NewGuid().ToString() },
                new ChatRoom { Name = "Random", Id = Guid.NewGuid().ToString() }
            };

            var messages = new[]
            {
                new Message
                {
                    Content = "Hello everyone!",
                    UserId = users[0].Id,
                    ChatRoomId = rooms[0].Id,
                    Timestamp = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString()
                }
            };

            context.Users.AddRange(users);
            context.ChatRooms.AddRange(rooms);
            context.Messages.AddRange(messages);
            context.SaveChanges();
        }
    }
}
