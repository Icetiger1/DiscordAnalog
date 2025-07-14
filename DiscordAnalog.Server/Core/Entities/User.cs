namespace DiscordAnalog.Server.API.Extensions
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
