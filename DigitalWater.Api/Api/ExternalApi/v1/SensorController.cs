using DigitalWater.Api.Attributes;
using DigitalWater.Core.Dto;
using DigitalWater.Core.Model;
using DigitalWater.Core.Repositories;
using DigitalWater.Data.Services;
using Microsoft.AspNetCore.Mvc;
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
    /// Получить информацию с датчиков
    /// </summary>
    /// <param name="request">параметры запроса</param>
    /// <returns>информация с датчиков</returns>
    [HttpGet]
    [SwaggerResponse(200, "Получить информацию с датчиков", typeof(List<Sensor>))]
    [SwaggerResponse(400, "Неверный запрос")]
    [SwaggerResponse(500, "Произошла ошибка при получении записей")]
    public async Task<ActionResult<List<Sensor>>> GetSensors([FromQuery] GetSensorsRequest request)
    {
        return await _serviceReceivingService.GetSensorsAsync(request);
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
    public ActionResult<Sensor> AddSensor([FromBody] Sensor sensor)
    {
        try
        {
            var found = _repository.Get(sensor.Id);
            if (found != null)
                return Conflict("Запись с таким id уже существует");
            
            _repository.Add(sensor);
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
    public ActionResult<Sensor> DeleteSensor([FromRoute] string id)
    {
        try
        {
            _repository.Remove(id);
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
    public ActionResult<Sensor> UpdateSensor([FromRoute] string id, [FromBody] Sensor sensor)
    {
        try
        {
            var found = _repository.Get(id);
            if (found == null)
                return BadRequest("Датчик не найден");
            if (id != sensor.Id)
                return BadRequest("Неверный id записи");

            _repository.Update(sensor);
            return Ok(sensor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}