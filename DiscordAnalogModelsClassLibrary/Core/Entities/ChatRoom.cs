
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordAnalogModelsClassLibrary.Core.Entities;

/// <summary> 
/// Текстовый канал/комната 
/// </summary>
[Table("chatroom")]
public class ChatRoom : BaseEntity
{
    /// <summary>
    /// Имя канала
    /// </summary>
    [Column("Name")]
    public string Name { get; set; } = string.Empty;
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}

