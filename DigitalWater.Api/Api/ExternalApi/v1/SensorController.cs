﻿using DigitalWater.Api.Attributes;
using DigitalWater.Core.Dto;
using DigitalWater.Core.Model;
using DigitalWater.Core.Repositories;
using DigitalWater.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DigitalWater.Api.Api.ExternalApi.v1;

/// <summary>
/// Контроллер для работы с датчиками
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
public class SensorController : ControllerBase
{
    private readonly IEfCoreRepository<Sensor> _repository;
    private readonly ServiceReceivingService _serviceReceivingService;

    public SensorController(IEfCoreRepository<Sensor> repository, ServiceReceivingService serviceReceivingService)
    {
        _repository = repository;
        _serviceReceivingService = serviceReceivingService;
    }

    /// <summary>
    /// Результат получения информации с датчиков
    /// </summary>
    /// <param name="TotalCount">общее количество записей</param>
    /// <param name="Data">текущая выборка</param>
    public record GetSensorsResponse(int TotalCount, List<Sensor> Data);

    /// <summary>
    /// Получить информацию с датчиков
    /// </summary>
    /// <param name="request">параметры запроса</param>
    /// <returns>информация с датчиков</returns>
    [HttpGet]
    [SwaggerResponse(200, "Получить информацию с датчиков", typeof(GetSensorsResponse))]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(500, "Произошла ошибка при получении записей")]
    public async Task<ActionResult<GetSensorsResponse>> GetSensors([FromQuery] GetSensorsRequest request)
    {
        var data = await _serviceReceivingService.GetSensorsAsync(request);
        return Ok(new GetSensorsResponse(data.Item2, data.Item1));
    }

    /// <summary>
    /// Добавить запись с информацией с датчика
    /// </summary>
    /// <param name="sensor">информация с датчика</param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(200, "Добавить информацию о датчике в базу данных")]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(409, "Запись с таким id уже существует")]
    [SwaggerResponse(500, "Произошла ошибка при добавлении записей")]
    public async Task<ActionResult<Sensor>> AddSensor([FromBody] Sensor sensor)
    {
        try
        {
            var found = _repository.Get(sensor.Id);
            if (found != null)
                return Conflict("Запись с таким id уже существует");
            
            _repository.Add(sensor);
            await _repository.SaveChangesAsync();
            return Ok(sensor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Удалить запись с информацией с датчика
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Удалить информацию о датчике из базы данных")]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(500, "Произошла ошибка при удалении записей")]
    public async Task<ActionResult<Sensor>> DeleteSensor([FromRoute] string id)
    {
        try
        {
            var found = _repository.Get(id);
            if (found == null)
                return Ok();
            
            _repository.Delete(found);
            await _repository.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Обновить запись с информацией с датчика
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="sensor">информация с датчика</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [SwaggerResponse(200, "Обновить информацию о датчике в базе данных")]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(500, "Произошла ошибка при обновлении записей")]
    public async Task<ActionResult<Sensor>> UpdateSensor([FromRoute] string id, [FromBody] Sensor sensor)
    {
        try
        {
            var found = _repository.GetListQuery().AsTracking().FirstOrDefault(p => p.Id == id);
            if (found == null)
                return BadRequest("Датчик не найден");
            if (id != sensor.Id)
                return BadRequest("Неверный id записи");
            
            // update found from sensor
            found.Location = sensor.Location;
            found.Type = sensor.Type;
            found.Readings = sensor.Readings;
            found.Status = sensor.Status;
            found.Alerts = sensor.Alerts;
            found.Metadata = sensor.Metadata;

            _repository.Update(found);
            await _repository.SaveChangesAsync();
            return Ok(found);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}