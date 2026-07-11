using System.Text.Json.Serialization;

namespace BL.DTOs.Shipment
{
    public class UpdateShipmentDto
    {
        public DateTime ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ShippingTypeId { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public decimal PackageValue { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public Guid? UserSubscriptionId { get; set; }
        public Guid? ShippingPackagingId { get; set; }
        public Guid? ReferenceId { get; set; }


        [JsonIgnore]
        public string? TrackingNumber { get; set; }
        [JsonIgnore]
        public decimal ShippingRate { get; set; }
    }
}
