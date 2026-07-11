using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TbSubscriptionPackage : BaseEntity
{
    public string PackageName { get; set; } = null!;

    public int ShipmentCount { get; set; }

    public double NumberOfKiloMeters { get; set; }

    public double TotalWeight { get; set; }

    public decimal Price { get; set; }  // Subscription price

    public int DurationDays { get; set; } // Default 30 days



    public virtual ICollection<TbUserSubscription> TbUserSubscriptions { get; set; } = new List<TbUserSubscription>();
}
