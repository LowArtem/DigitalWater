namespace DigitalWater.Core.Model;

/// <summary>
/// Алерты
/// </summary>
public class Alert
{
    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public bool Resolved { get; set; }
}