using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.Models.Shipment;

public class CreateShipmentViewModel
{
    [Display(Name = "Packaging Type")]
    public Guid? ShippingPackagingId { get; set; }

    [Required(ErrorMessage = "Please select a shipping type.")]
    [Display(Name = "Shipping Type")]
    public Guid ShippingTypeId { get; set; }

    [Required(ErrorMessage = "Shipping date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Shipping Date")]
    public DateTime ShippingDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Delivery Date")]
    public DateTime? DeliveryDate { get; set; }

    [Required(ErrorMessage = "Weight is required.")]
    [Range(0.01, 99999, ErrorMessage = "Weight must be greater than zero.")]
    [Display(Name = "Weight (kg)")]
    public double Weight { get; set; }

    [Range(0, 99999, ErrorMessage = "Length must be zero or greater.")]
    [Display(Name = "Length (cm)")]
    public double Length { get; set; }

    [Range(0, 99999, ErrorMessage = "Width must be zero or greater.")]
    [Display(Name = "Width (cm)")]
    public double Width { get; set; }

    [Range(0, 99999, ErrorMessage = "Height must be zero or greater.")]
    [Display(Name = "Height (cm)")]
    public double Height { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Package value must be zero or greater.")]
    [Display(Name = "Declared Value (USD)")]
    public decimal PackageValue { get; set; }

    [Display(Name = "Payment Method")]
    public Guid? PaymentMethodId { get; set; }

    public Guid? UserSubscriptionId { get; set; }
    public Guid? ReferenceId { get; set; }

    // ── Dropdown sources (not submitted) ──
    public List<SelectListItem>? ShippingTypes { get; set; }
    public List<SelectListItem>? PackagingTypes { get; set; }
    public List<SelectListItem>? PaymentMethods { get; set; }
}