using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.SharedSenderReceiver;

public abstract class SharedUpdateSenderReceiverDto
{
    public string? UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Contact { get; set; } = null!;
    public string? OtherAddress { get; set; }
    public bool IsDefaultAddress { get; set; }
    public Guid CityId { get; set; }
    public string Address { get; set; } = null!;
}