using DiscordAnalog.Server.Core.Entities;

namespace DiscordAnalog.Server.API.Extensions
{
    public class Message : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;
    }
}
