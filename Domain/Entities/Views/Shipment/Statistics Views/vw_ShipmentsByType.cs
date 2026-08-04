using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Shipment.Statistics_Views;

public class VwShipmentsByType
{
    public Guid ShippingTypeId { get; set; }
    public string? ShippingTypeName { get; set; }

    // ✅ تمت إضافة الحالة
    public enShipmentStatus Status { get; set; }
    public string? StatusName { get; set; }

    public int ShipmentCount { get; set; }
    public decimal? TotalPackageValue { get; set; }
    public decimal? TotalShippingRate { get; set; }
    public decimal? AverageShippingRate { get; set; }
}