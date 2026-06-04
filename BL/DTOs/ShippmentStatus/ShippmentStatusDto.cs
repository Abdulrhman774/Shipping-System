using BL.DTOs.Base;

namespace BL.DTOs.ShippmentStatus
{
    public class ShippmentStatusDto : BaseDto
    {
        public Guid? ShippmentId { get; set; }
        public string? Notes { get; set; }
        public Guid CarrierId { get; set; }
    }
}
