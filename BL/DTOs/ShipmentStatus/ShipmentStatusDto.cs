using BL.DTOs.Base;

namespace BL.DTOs.ShipmentStatus
{
    public class ShipmentStatusDto : BaseDto
    {
        public Guid? ShipmentId { get; set; }
        public string? Notes { get; set; }
        public Guid CarrierId { get; set; }
    }
}
