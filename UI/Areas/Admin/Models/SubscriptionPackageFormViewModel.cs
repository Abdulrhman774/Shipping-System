namespace UI.Areas.Admin.Models;

public class SubscriptionPackageFormViewModel
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int ShipimentCount { get; set; }
    public double NumberOfKiloMeters { get; set; }
    public double TotalWeight { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEdit => Id != Guid.Empty;
}
