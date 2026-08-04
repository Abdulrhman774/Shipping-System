using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Views.Shipment.Statistics_Views;

public class vw_ShipmentDetails
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
    public Guid SenderId { get; set; }
    public string? SenderName { get; set; }
    public string? SenderEmail { get; set; }
    public string? SenderPhone { get; set; }
    public string? SenderAddress { get; set; }
    public string? SenderPostalCode { get; set; }
    public string? SenderContact { get; set; }
    public string? SenderOtherAddress { get; set; }
    public bool? SenderIsDefault { get; set; }
    public Guid? SenderCityId { get; set; }
    public string? SenderCityName { get; set; }
    public string? SenderCityAName { get; set; }
    public Guid? SenderCountryId { get; set; }
    public string? SenderCountryName { get; set; }
    public string? SenderCountryAName { get; set; }

    // المستلم (Receiver)
    public Guid ReceiverId { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverEmail { get; set; }
    public string? ReceiverPhone { get; set; }
    public string? ReceiverAddress { get; set; }
    public string? ReceiverPostalCode { get; set; }
    public string? ReceiverContact { get; set; }
    public string? ReceiverOtherAddress { get; set; }
    public bool? ReceiverIsDefault { get; set; }
    public Guid? ReceiverCityId { get; set; }
    public string? ReceiverCityName { get; set; }
    public string? ReceiverCityAName { get; set; }
    public Guid? ReceiverCountryId { get; set; }
    public string? ReceiverCountryName { get; set; }
    public string? ReceiverCountryAName { get; set; }

    // نوع الشحن
    public Guid ShippingTypeId { get; set; }
    public string? ShippingTypeName { get; set; }
    public string? ShippingTypeAName { get; set; }
    public double? ShippingFactor { get; set; }

    // طريقة الدفع
    public string? PaymentMethodAName { get; set; }
    public string? PaymentMethodEName { get; set; }
    public double? PaymentCommission { get; set; }

    // التغليف
    public string? PackagingAName { get; set; }
    public string? PackagingEName { get; set; }

    // بيانات الحالة (OLD)
    public enEntityState CurrentState { get; set; }

    // ✅ الحقول الجديدة
    public enShipmentStatus Status { get; set; }           // تم إضافتها
    public DateTime? StatusLastUpdatedAt { get; set; }    // تم إضافتها

    // بيانات التدقيق
    public DateTime CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
}