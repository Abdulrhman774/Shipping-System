using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public partial class TbShippingPackaging : BaseEntity
{
    public string ShippingPackagingAname { get; set; } = null!;

    public string ShippingPackagingEname { get; set; } = null!;

    public virtual ICollection<TbShipment> TbShipments { get; set; } = new List<TbShipment>();
}

