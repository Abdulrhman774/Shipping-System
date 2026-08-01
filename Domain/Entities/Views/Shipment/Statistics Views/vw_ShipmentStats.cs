using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Shipment.Statistics_Views;

public class vw_ShipmentStats
{
    public enEntityState CurrentState { get; set; }
    public string? StatusName { get; set; }
    public int ShipmentCount { get; set; }
    public decimal? TotalPackageValue { get; set; }
    public decimal? TotalShippingRate { get; set; }
    public decimal? AverageShippingRate { get; set; }
    public DateTime? OldestShipment { get; set; }
    public DateTime? LatestShipment { get; set; }
}
