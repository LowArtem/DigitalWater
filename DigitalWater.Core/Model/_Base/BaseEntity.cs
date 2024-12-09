using System.ComponentModel.DataAnnotations;

namespace DigitalWater.Core.Model._Base;

/// <summary>
/// Базовый класс для сущностей БД
/// </summary>
public class BaseEntity : IEntity
{
    /// <inheritdoc />
    [Key]
    public int Id { get; set; }
}