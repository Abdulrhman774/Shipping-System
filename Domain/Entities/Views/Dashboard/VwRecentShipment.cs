using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Dashboard;

public class VwRecentShipment
{
    public Guid ShipmentId { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Destination { get; set; }
    public enShipmentStatus Status { get; set; }
    public string? Priority { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime CreatedDate { get; set; }
}