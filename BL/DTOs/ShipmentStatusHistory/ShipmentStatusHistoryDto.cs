using BL.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.ShipmentStatusHistory;

public class ShipmentStatusHistoryDto : BaseDto
{
    public Guid ShipmentId { get; set; }
    public enShipmentStatus Status { get; set; }
    public string? Note { get; set; }
    public Guid? CreatedBy { get; set; }
}
