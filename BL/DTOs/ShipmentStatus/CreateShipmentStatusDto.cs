namespace BL.DTOs.ShipmentStatus
{
    public class CreateShipmentStatusDto
    {
        public Guid? ShipmentId { get; set; }
        public string? Notes { get; set; }
        public Guid CarrierId { get; set; }
    }
}
