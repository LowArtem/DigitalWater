using DigitalWater.Core.Dto;
using DigitalWater.Core.Model;
using DigitalWater.Core.Repositories;

namespace DigitalWater.Data.Services;

/// <summary>
/// Сервис получения данных
/// </summary>
public class ServiceReceivingService
{
    private readonly IEfCoreRepository<Sensor> _repository;

    public ServiceReceivingService(IEfCoreRepository<Sensor> repository)
    {
        _repository = repository;
    }

    public async Task<List<Sensor>> GetSensorsAsync(GetSensorsRequest request)
    {
        return new List<Sensor>();
    }
}