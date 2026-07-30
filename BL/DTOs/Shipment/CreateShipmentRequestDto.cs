using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Shipment;

public class CreateShipmentRequestDto
{
    public CreateShipmentDto ShipmentDto { get; set; } = default!;
    public CreateUserSenderDto SenderDto { get; set; } = default!;
    public CreateUserReceiverDto ReceiverDto { get; set; } = default!;
}
