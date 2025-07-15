using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordAnalogModelsClassLibrary.Core.Entities;

/// <summary>
/// Сообщение
/// </summary>
[Table("message")]
public class Message : BaseEntity
{
    /// <summary>
    /// Текст сообщения
    /// </summary>
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время сообщения
    /// </summary>
    [Column("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Id пользователя автора сообщения
    /// </summary>
    [Column("userid")]
    public string UserId { get; set; }
    public User User { get; set; } = null!;

    /// <summary>
    /// Id канада
    /// </summary>
    [Column("chatroomid")]
    public string ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
}

