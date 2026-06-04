using Domain.Shared;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TbCity : BaseEntity
{
    public string? CityAname { get; set; }

    public string? CityEname { get; set; }

    public Guid CountryId { get; set; }

    public virtual TbCountry Country { get; set; } = null!;

    public virtual ICollection<TbUserReceiver> TbUserReceivers { get; set; } = new List<TbUserReceiver>();

    public virtual ICollection<TbUserSender> TbUserSebders { get; set; } = new List<TbUserSender>();
}
