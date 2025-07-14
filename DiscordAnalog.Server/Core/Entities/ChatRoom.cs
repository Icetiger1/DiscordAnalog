using DiscordAnalog.Server.API.Extensions;

namespace DiscordAnalog.Server.Core.Entities
{
    /// <summary> Текстовый канал/комната </summary>
    public class ChatRoom : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
