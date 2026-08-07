namespace UI.Areas.Admin.Models;

public class ShippingTypeRowItem
{
    public Guid Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public string ShippingFactor { get; set; } = string.Empty;
    public enShipmentStatus Status { get; set; }
}
