namespace UI.Areas.Admin.Models;

public class CountryRowItem
{
    public Guid Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public enShipmentStatus Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
