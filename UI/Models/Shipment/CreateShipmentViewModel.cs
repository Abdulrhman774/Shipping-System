using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.Models.Shipment
{
    public class CreateShipmentViewModel
    {
        [Required] public ShipmentPartyViewModel Sender { get; set; } = new();
        [Required] public ShipmentPartyViewModel Receiver { get; set; } = new();

        public Guid? ShippingPackagingId { get; set; }

        [Required, Range(0.01, double.MaxValue)] public double Weight { get; set; }
        [Range(0, double.MaxValue)] public double Length { get; set; }
        [Range(0, double.MaxValue)] public double Width { get; set; }
        [Range(0, double.MaxValue)] public double Height { get; set; }
        [Range(0, double.MaxValue)] public decimal PackageValue { get; set; }

        [Required] public Guid ShippingTypeId { get; set; }
        [Required, DataType(DataType.Date)] public DateTime ShippingDate { get; set; } = DateTime.UtcNow;

        public Guid? PaymentMethodId { get; set; }
        public Guid? UserSubscriptionId { get; set; }

        // Populated by the Controller, not submitted by the user
        public List<SelectListItem>? Cities { get; set; }
        public List<SelectListItem>? ShippingTypes { get; set; }
        public List<SelectListItem>? PackagingTypes { get; set; }
        public List<SelectListItem>? PaymentMethods { get; set; }
    }

    public class ShipmentPartyViewModel
    {
        public Guid? ExistingId { get; set; } // if the user picks a previously saved address

        [Required] public string Name { get; set; } = string.Empty;
        public string? Contact { get; set; }
        [Required] public string Address { get; set; } = string.Empty;
        public string? OtherAddress { get; set; }
        [Required] public string PostalCode { get; set; } = string.Empty;
        [Required] public Guid CityId { get; set; }
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, Phone] public string Phone { get; set; } = string.Empty;
    }
}
