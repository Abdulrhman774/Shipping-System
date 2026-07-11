using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TbUserSubscription : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid PackageId { get; set; }

    public DateTime SubscriptionDate { get; set; }


    // Add tracking for subscription usage
    public int UsedShipmentCount { get; set; }
    public double UsedTotalWeight { get; set; }
    public double UsedTotalDistance { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }

    public virtual TbSubscriptionPackage Package { get; set; } = null!;
    public virtual ICollection<TbShipment> Shipments { get; set; } = new List<TbShipment>();

}
