namespace UI.Areas.Admin.Models;

public class ShippingTypeRowItem
{
    public Guid Id { get; set; }
    public string ShippingTypeEname { get; set; } = string.Empty;
    public string ShippingTypeAname { get; set; } = string.Empty;
    public double ShippingFactor { get; set; }
    public enEntityState Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
