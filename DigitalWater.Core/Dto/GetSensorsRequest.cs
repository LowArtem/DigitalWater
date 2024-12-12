using DigitalWater.Core.Enum;

namespace DigitalWater.Core.Dto;

/// <summary>
/// Данные для получения списка показаний датчиков
/// </summary>
/// <param name="From">Начальная позиция</param>
/// <param name="Count">Количество</param>
/// <param name="AddressSubstr">Подстрока адреса</param>
/// <param name="ObjectTypeSubstr">Подстрока типа объекта</param>
/// <param name="ValueMin">Минимальное значение показаний</param>
/// <param name="ValueMax">Максимальное значение показаний</param>
/// <param name="SensorId">Id датчика</param>
/// <param name="SortBy">Поле сортировки</param>
/// <param name="SortOrder">Порядок сортировки</param>
public record GetSensorsRequest(
    int From = 0,
    int Count = 10,
    string? AddressSubstr = null,
    string? ObjectTypeSubstr = null,
    double? ValueMin = null,
    double? ValueMax = null,
    string? SensorId = null,
    SortBy? SortBy = null,
    SortOrder SortOrder = SortOrder.Ascending);