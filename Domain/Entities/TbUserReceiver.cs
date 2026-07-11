using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TbUserReceiver : BaseEntity
{
    public string? UserId { get; set; }

    public string ReceiverName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string OtherAddress { get; set; } = null!;

    public bool IsDefaultAddress { get; set; }

    public Guid CityId { get; set; }

    public string Address { get; set; } = null!;

    public virtual TbCity City { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }  // Nullable for guest users
    public virtual ICollection<TbShipment> TbShippments { get; set; } = new List<TbShipment>();
}
