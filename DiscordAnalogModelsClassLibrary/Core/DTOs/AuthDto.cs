namespace DiscordAnalogModelsClassLibrary.Core.DTOs;

public class AuthDto
{
    public record AuthRequest(string Username, string Password);
    public record AuthResponse(string Token, string Username);

    public UserDto User { get; set; }


    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}

