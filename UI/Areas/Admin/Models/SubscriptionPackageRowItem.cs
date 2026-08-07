namespace UI.Areas.Admin.Models;

public class SubscriptionPackageRowItem
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public string Uid { get; set; } = string.Empty;
    public string ShipmentCount { get; set; } = string.Empty;
    public string DistanceKm { get; set; } = string.Empty;
    public string WeightKg { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string DurationDays { get; set; } = string.Empty;
    public enShipmentStatus Status { get; set; }
    public string Icon { get; set; } = "fa-cube";
}
