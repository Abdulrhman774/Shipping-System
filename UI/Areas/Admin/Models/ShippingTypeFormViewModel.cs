namespace UI.Areas.Admin.Models;

public class ShippingTypeFormViewModel
{
    public Guid Id { get; set; }
    public string ShippingTypeEname { get; set; } = string.Empty;
    public string ShippingTypeAname { get; set; } = string.Empty;
    public double ShippingFactor { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id != Guid.Empty;
}
