using DigitalWater.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWater.Api.Api.ExternalApi.v1;

/// <summary>
/// Контроллер для работы с датчиками
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
public class SensorController : ControllerBase
{
    /// <summary>
    /// Пинг
    /// </summary>
    /// <returns></returns>
    [HttpGet("hello")]
    public IActionResult Ping() => Ok("Hello");
}