namespace UI.Areas.Admin.Models;

public class UserSubscriptionRowItem
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string SubId { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public string PackageColor { get; set; } = "#5e5ce6";
    public string SubscribedDate { get; set; } = string.Empty;
    public int UsedShipments { get; set; }
    public int TotalShipments { get; set; }
    public string ExpiryDate { get; set; } = string.Empty;
    public enShipmentStatus Status { get; set; }
}
