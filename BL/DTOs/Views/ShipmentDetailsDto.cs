// BL/DTOs/Views/ShipmentDetailsDto.cs
namespace BL.DTOs.Views
{
    public class ShipmentDetailsDto
    {
        // بيانات الشحنة الأساسية
        public Guid ShipmentId { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public decimal PackageValue { get; set; }
        public decimal ShippingRate { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public Guid? UserSubscriptionId { get; set; }
        public Guid? ShippingPackagingId { get; set; }
        public Guid? ReferenceId { get; set; }

        // المرسل (Sender)
        public Guid? SenderCityId { get; set; }        // 👈 جديد
        public Guid SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderPhone { get; set; }
        public string? SenderAddress { get; set; }
        public string? SenderPostalCode { get; set; }
        public string? SenderContact { get; set; }
        public string? SenderOtherAddress { get; set; }
        public bool? SenderIsDefault { get; set; }
        public string? SenderCityName { get; set; }
        public string? SenderCountryName { get; set; }

        // المستلم (Receiver)
        public Guid? ReceiverCityId { get; set; }      // 👈 جديد
        public Guid ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverEmail { get; set; }
        public string? ReceiverPhone { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPostalCode { get; set; }
        public string? ReceiverContact { get; set; }
        public string? ReceiverOtherAddress { get; set; }
        public bool? ReceiverIsDefault { get; set; }
        public string? ReceiverCityName { get; set; }
        public string? ReceiverCountryName { get; set; }

        // نوع الشحن
        public Guid ShippingTypeId { get; set; }
        public string? ShippingTypeName { get; set; }

        // ✅ طريقة الدفع (جديد)
        public string? PaymentMethodAName { get; set; }
        public string? PaymentMethodEName { get; set; }

        // ✅ التغليف (جديد)
        public string? PackagingAName { get; set; }
        public string? PackagingEName { get; set; }

        // بيانات الحالة
        public enEntityState CurrentState { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}