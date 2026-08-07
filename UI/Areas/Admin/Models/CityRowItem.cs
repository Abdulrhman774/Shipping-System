namespace UI.Areas.Admin.Models;

public class CityRowItem
{
    public Guid Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public enShipmentStatus Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
