namespace UI.Areas.Admin.Models;

public class SubscriptionPackageFormViewModel
{
    public Guid? Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int ShipmentCount { get; set; }
    public double NumberOfKiloMeters { get; set; }
    public double TotalWeight { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; } = 30;
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id.HasValue;
}
