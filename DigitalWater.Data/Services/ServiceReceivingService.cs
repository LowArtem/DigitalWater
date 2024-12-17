using DigitalWater.Core.Dto;
using DigitalWater.Core.Enum;
using DigitalWater.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace DigitalWater.Data.Services;

/// <summary>
/// Сервис получения данных
/// </summary>
public class ServiceReceivingService
{
    private readonly ApplicationContext _context;

    public ServiceReceivingService(ApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить информацию с датчиков
    /// </summary>
    /// <param name="request">параметры запроса</param>
    /// <returns></returns>
    public async Task<(List<Sensor>, int)> GetSensorsAsync(GetSensorsRequest request)
    {
        var query = _context.Sensors.AsQueryable();

        // Фильтрация по подстроке адреса
        if (!string.IsNullOrEmpty(request.AddressSubstr))
        {
            query = query.Where(sensor => sensor.Location.Address.Contains(request.AddressSubstr));
        }

        // Фильтрация по подстроке типа объекта
        if (!string.IsNullOrEmpty(request.ObjectTypeSubstr))
        {
            query = query.Where(sensor => sensor.Location.ObjectType.Contains(request.ObjectTypeSubstr));
        }

        // Фильтрация по идентификатору датчика
        if (!string.IsNullOrEmpty(request.SensorId))
        {
            query = query.Where(sensor => sensor.Id.Contains(request.SensorId));
        }

        // Предварительная фильтрация по наличию Readings в диапазоне
        if (request.ValueMin.HasValue || request.ValueMax.HasValue)
        {
            query = query.Where(sensor => sensor.Readings.Any(reading =>
                (!request.ValueMin.HasValue || reading.Value >= request.ValueMin.Value) &&
                (!request.ValueMax.HasValue || reading.Value <= request.ValueMax.Value)));
        }

        // Сортировка
        if (request.SortBy.HasValue)
        {
            query = request.SortBy switch
            {
                SortBy.Manufacturer => request.SortOrder == SortOrder.Descending
                    ? query.OrderByDescending(sensor => sensor.Metadata.Manufacturer)
                    : query.OrderBy(sensor => sensor.Metadata.Manufacturer),
                SortBy.SensorId => request.SortOrder == SortOrder.Descending
                    ? query.OrderByDescending(sensor => sensor.Id)
                    : query.OrderBy(sensor => sensor.Id),
                SortBy.AlertsCount => request.SortOrder == SortOrder.Descending
                    ? query.OrderByDescending(sensor => sensor.Alerts.Count)
                    : query.OrderBy(sensor => sensor.Alerts.Count),
                _ => query
            };
        }
        
        var totalCount = await query.CountAsync();

        // Выполнение запроса и ручная фильтрация Readings
        var sensors = await query
            .Skip(request.From)
            .Take(request.Count)
            .ToListAsync();

        foreach (var sensor in sensors)
        {
            sensor.Readings = sensor.Readings
                .Where(reading => (!request.ValueMin.HasValue || reading.Value >= request.ValueMin.Value) &&
                                  (!request.ValueMax.HasValue || reading.Value <= request.ValueMax.Value))
                .ToList();
        }

        return (sensors, totalCount);
    }
}