using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordAnalogModelsClassLibrary.Core.Entities;

public abstract class BaseEntity
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    [Column("id")]
    public string Id { get; set; }
}

