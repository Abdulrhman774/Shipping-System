using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.Models;

public class CreateShipmentViewModel
{
    [Required]
    public ShipmentPartyViewModel Sender { get; set; } = new();

    [Required]
    public ShipmentPartyViewModel Receiver { get; set; } = new();

    public Guid? ShippingPackagingId { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0")]
    public double Weight { get; set; }

    [Range(0, double.MaxValue)]
    public double Length { get; set; }

    [Range(0, double.MaxValue)]
    public double Width { get; set; }

    [Range(0, double.MaxValue)]
    public double Height { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PackageValue { get; set; }

    [Required(ErrorMessage = "Please select a shipping type")]
    public Guid ShippingTypeId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime ShippingDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.Date)]
    public DateTime DeliveryDate { get; set; } = DateTime.UtcNow;

    public Guid? PaymentMethodId { get; set; }

    public Guid? UserSubscriptionId { get; set; }

    // Dropdown sources (تتملى من الـ Controller)
    public List<SelectListItem>? Cities { get; set; }
    public List<SelectListItem>? ShippingTypes { get; set; }
    public List<SelectListItem>? PackagingTypes { get; set; }
    public List<SelectListItem>? PaymentMethods { get; set; }
}

public class ShipmentPartyViewModel
{
    public Guid? ExistingId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    public string? Contact { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; } = string.Empty;

    public string? OtherAddress { get; set; }

    [Required(ErrorMessage = "Postal Code is required")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telephone is required")]
    [Phone]
    public string Phone { get; set; } = string.Empty;
}