namespace DigitalWater.Core.Model._Base;

/// <summary>
/// Интерфейс сущности БД
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public string Id { get; set; }
}