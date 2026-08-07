namespace Domain.Entities.Views.Dashboard;

public class VwTopShipper
{
    public Guid SenderId { get; set; }
    public string? SenderName { get; set; }
    public int ShipmentCount { get; set; }
    public decimal TotalRevenue { get; set; }
}