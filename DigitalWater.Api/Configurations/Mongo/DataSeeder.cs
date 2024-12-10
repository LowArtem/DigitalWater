using DigitalWater.Core.Model;
using DigitalWater.Data;
using Microsoft.Extensions.Options;

namespace DigitalWater.Api.Configurations.Mongo;

/// <summary>
/// Класс для заполнения БД начальными данными
/// </summary>
public class DataSeeder
{
    private readonly List<Sensor>? _sensors;
    
    public DataSeeder(IOptions<List<Sensor>> sensors)
    {
        _sensors = sensors?.Value ?? [];
    }
    
    /// <summary>
    /// Заполнить БД начальными данными
    /// </summary>
    /// <param name="host"></param>
    public void SeedDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        // Проверяем, есть ли данные в коллекции
        if (!context.Sensors.Any())
        {
            // если нет данных для заполнения
            if (_sensors == null || _sensors.Count == 0)
            {
                Console.WriteLine("Файл initData.json не найден или пуст. Пропуск инициализации данных.");
                return;
            }
            
            context.Sensors.AddRange(_sensors);
            context.SaveChanges();
            Console.WriteLine("Начальные данные успешно загружены в базу данных.");
        }
        else
        {
            Console.WriteLine("База данных уже содержит данные. Инициализация пропущена.");
        }
    }
}