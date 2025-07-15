using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordAnalogModelsClassLibrary.Core.Entities;

/// <summary>
/// Пользователя
/// </summary>
[Table("users")]
public class User : BaseEntity
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля пользователя
    /// </summary>
    [Column("passwordhash")]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Список сообщений пользователя
    /// </summary>
    public ICollection<Message> Messages { get; set; } = new List<Message>();

}

