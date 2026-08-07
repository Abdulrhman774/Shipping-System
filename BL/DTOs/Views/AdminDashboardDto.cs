using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Views;

public class AdminDashboardDto
{
    public int TotalShipments { get; set; }
    public int ActiveDeliveries { get; set; }
    public int PendingApprovals { get; set; }
    public double EfficiencyRate { get; set; }
    public int DelayedShipments { get; set; }
    public List<ShipmentDetailsDto> RecentShipments { get; set; } = new();
}
