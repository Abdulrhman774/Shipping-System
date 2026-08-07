namespace Domain.Entities.Views.Dashboard;

public class VwDashboardSummary
{
    public int TotalShipments { get; set; }
    public int ActiveDeliveries { get; set; }
    public int PendingApprovals { get; set; }
    public int DelayedShipments { get; set; }
    public double EfficiencyRate { get; set; }
}