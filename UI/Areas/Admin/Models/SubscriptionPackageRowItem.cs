namespace UI.Areas.Admin.Models;

public class SubscriptionPackageRowItem
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int ShipimentCount { get; set; }
    public double NumberOfKiloMeters { get; set; }
    public double TotalWeight { get; set; }
    public enEntityState Status { get; set; }
    public string CreatedDate { get; set; } = string.Empty;
}
