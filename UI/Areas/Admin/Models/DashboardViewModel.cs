namespace UI.Areas.Admin.Models;

public class DashboardViewModel
{
    public int TotalShipments { get; set; }
    public int ActiveDeliveries { get; set; }
    public int PendingApprovals { get; set; }
    public string EfficiencyRate { get; set; } = string.Empty;
    public int DelayedShipments { get; set; }
    public List<RecentShipmentItem> RecentShipments { get; set; } = new();
}

public class RecentShipmentItem
{
    public string Id { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string LastUpdate { get; set; } = string.Empty;
}
