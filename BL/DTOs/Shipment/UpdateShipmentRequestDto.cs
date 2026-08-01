using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Shipment;

public class UpdateShipmentRequestDto
{
    public UpdateShipmentDto ShipmentDto { get; set; } = default!;
    public UpdateUserSenderDto SenderDto { get; set; } = default!;
    public UpdateUserReceiverDto ReceiverDto { get; set; } = default!;
}
