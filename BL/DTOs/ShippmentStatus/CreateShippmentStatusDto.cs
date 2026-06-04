namespace BL.DTOs.ShippmentStatus
{
    public class CreateShippmentStatusDto
    {
        public Guid? ShippmentId { get; set; }
        public string? Notes { get; set; }
        public Guid CarrierId { get; set; }
    }
}
