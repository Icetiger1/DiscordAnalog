using DiscordAnalogModelsClassLibrary.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordAnalog.Server.Infrastructure.Data
{
    /// <summary> 
    /// Контекст БД (поддерживает PostgreSQL и другие провайдеры) 
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Схема БД
        /// </summary>
        public static readonly string Schema = "public";

        /// <summary>
        /// Конструктор AppDbContext
        /// </summary>
        /// <param name="options">base</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// ChatRooms DbSet
        /// </summary>
        public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();

        /// <summary>
        /// Users DbSet
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Messages DbSet
        /// </summary>
        public DbSet<Message> Messages => Set<Message>();

        /// <summary>
        ///  
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly,
                t => t.Namespace == typeof(Message).Namespace
            );

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

        }
    }
}
