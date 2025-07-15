namespace DiscordAnalogModelsClassLibrary.Core.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
