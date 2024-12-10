namespace DigitalWater.Core.Model;

/// <summary>
/// Показания
/// </summary>
public class Reading
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public string Unit { get; set; }
}