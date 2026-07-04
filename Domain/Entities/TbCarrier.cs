using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TbCarrier : BaseEntity
{
    public string CarrierName { get; set; } = null!;

    public virtual ICollection<TbShipmentStatus> TbShippmentStatuses { get; set; } = new List<TbShipmentStatus>();
}
