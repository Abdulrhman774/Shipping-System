using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.ShipmentStatusHistory;

public class UpdateShipmentStatusHistoryDto
{
    public Guid Id { get; set; }
    public Guid ShipmentId { get; set; }
    public enShipmentStatus Status { get; set; }
    public string? Note { get; set; }
}
