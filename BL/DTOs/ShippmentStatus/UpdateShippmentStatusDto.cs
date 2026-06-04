namespace BL.DTOs.ShippmentStatus
{
    public class UpdateShippmentStatusDto
    {
        public Guid? ShippmentId { get; set; }
        public string? Notes { get; set; }
        public Guid CarrierId { get; set; }
    }
}
