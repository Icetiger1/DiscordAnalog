namespace DiscordAnalog.Server.Core.DTOs
{
    public class AuthDtos
    {
        public record AuthRequest(string Username, string Password);
        public record AuthResponse(string Token, string Username);
    }
}
