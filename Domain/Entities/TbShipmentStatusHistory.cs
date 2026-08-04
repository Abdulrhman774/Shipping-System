using Domain.Shared;

namespace Domain.Entities
{
    public class TbShipmentStatusHistory : BaseEntity
    {
        public TbShipment Shipment { get; set; } = null!;
        public enShipmentStatus Status { get; set; }
        public string? Note { get; set; }

        public Guid ShipmentId { get; set; }
    }
}
