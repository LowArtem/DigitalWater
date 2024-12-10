namespace DigitalWater.Core.Model;

/// <summary>
/// Местоположение
/// </summary>
public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
    public string ObjectType { get; set; }
}