using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TbShipmentStatus : BaseEntity
{
    public Guid? ShipmentId { get; set; }

    public string? Notes { get; set; }

    public Guid CarrierId { get; set; }

    public virtual TbCarrier Carrier { get; set; } = null!;

    public virtual TbShipment? Shippment { get; set; }
}
