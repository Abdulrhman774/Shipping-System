using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Shipment.Statistics_Views;

public class VwMonthlyShipments
{
    public int ShipmentYear { get; set; }
    public int ShipmentMonth { get; set; }
    public string? MonthName { get; set; }

    // ✅ تمت إضافة الحالة
    public enShipmentStatus Status { get; set; }
    public string? StatusName { get; set; }

    public int TotalShipments { get; set; }
    public decimal? TotalValue { get; set; }
    public decimal? TotalRevenue { get; set; }
    public decimal? AverageRate { get; set; }
}