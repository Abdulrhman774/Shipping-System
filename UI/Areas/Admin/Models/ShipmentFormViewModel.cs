using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.Areas.Admin.Models
{
    public class ShipmentFormViewModel
    {
        // ==============================
        // Shipment IDs
        // ==============================
        public Guid ShipmentId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }

        // ==============================
        // Sender Fields
        // ==============================
        [Required] public string SenderName { get; set; } = string.Empty;
        [Required, EmailAddress] public string SenderEmail { get; set; } = string.Empty;
        [Required, Phone] public string SenderPhone { get; set; } = string.Empty;
        [Required] public string SenderAddress { get; set; } = string.Empty;
        public string? SenderOtherAddress { get; set; }
        [Required] public string SenderPostalCode { get; set; } = string.Empty;
        [Required] public string SenderContact { get; set; } = string.Empty;
        [Required] public Guid SenderCityId { get; set; }
        public bool SenderIsDefault { get; set; }

        // ==============================
        // Receiver Fields
        // ==============================
        [Required] public string ReceiverName { get; set; } = string.Empty;
        [Required, EmailAddress] public string ReceiverEmail { get; set; } = string.Empty;
        [Required, Phone] public string ReceiverPhone { get; set; } = string.Empty;
        [Required] public string ReceiverAddress { get; set; } = string.Empty;
        public string? ReceiverOtherAddress { get; set; }
        [Required] public string ReceiverPostalCode { get; set; } = string.Empty;
        [Required] public string ReceiverContact { get; set; } = string.Empty;
        [Required] public Guid ReceiverCityId { get; set; }
        public bool ReceiverIsDefault { get; set; }

        // ==============================
        // Shipment Details Fields
        // ==============================
        [Required] public DateTime ShippingDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        [Required] public Guid ShippingTypeId { get; set; }
        public Guid? ShippingPackagingId { get; set; }
        [Required] public double Weight { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        [Required] public decimal PackageValue { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public Guid? UserSubscriptionId { get; set; }
        public Guid? ReferenceId { get; set; }

        // ==============================
        // Dropdowns (Populated by Controller)
        // ==============================
        public List<SelectListItem> Cities { get; set; } = new();
        public List<SelectListItem> ShippingTypes { get; set; } = new();
        public List<SelectListItem> PackagingTypes { get; set; } = new();
        public List<SelectListItem> PaymentMethods { get; set; } = new();
    }
}