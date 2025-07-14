using DiscordAnalog.Server.API.Extensions;
using DiscordAnalog.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Data
{
    /// <summary> Контекст БД (поддерживает PostgreSQL и другие провайдеры) </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Добавляем DbSet 
        public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация отношений
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ChatRoom)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // При необходимости можно добавить начальные данные
            modelBuilder.Entity<ChatRoom>().HasData(
                new ChatRoom { Id = 1, Name = "General" },
                new ChatRoom { Id = 2, Name = "Random" }
            );
        }
    }
}
