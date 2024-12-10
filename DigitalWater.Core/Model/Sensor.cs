using DigitalWater.Core.Model._Base;

namespace DigitalWater.Core.Model;

/// <summary>
/// Датчик
/// </summary>
public class Sensor : BaseEntity
{
    public Location Location { get; set; }

    public string Type { get; set; }

    public List<Reading> Readings { get; set; }

    public string Status { get; set; }
    
    public List<Alert> Alerts { get; set; }

    public Metadata Metadata { get; set; }
}