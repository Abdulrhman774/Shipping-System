namespace UI.Areas.Admin.Models;

public class ShippingTypeFormViewModel
{
    public Guid? Id { get; set; }
    public string EnglishName { get; set; } = string.Empty;
    public string ArabicName { get; set; } = string.Empty;
    public double ShippingFactor { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
}
