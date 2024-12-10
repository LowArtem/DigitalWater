namespace DigitalWater.Core.Model;

/// <summary>
/// Метаданные
/// </summary>
public class Metadata
{
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public DateTime InstallationDate { get; set; }
    public DateTime WarrantyExpiration { get; set; }
}