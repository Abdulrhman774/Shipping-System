namespace BL.DTOs.ShipmentStatus
{
    public class UpdateShipmentStatusDto
    {
        public Guid? ShipmentId { get; set; }
        public string? Notes { get; set; }
        public Guid CarrierId { get; set; }
    }
}
