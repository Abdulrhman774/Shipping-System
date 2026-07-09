using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.Models.Shipment;

public class CreateShipmentViewModel
{
    public Guid? ShippingPackagingId { get; set; }

    [Required]
    public Guid ShippingTypeId { get; set; }

    [Required]
    public DateTime ShippingDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [Required]
    public double Weight { get; set; }

    public double Length { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public decimal PackageValue { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? UserSubscriptionId { get; set; }

    public List<SelectListItem>? ShippingTypes { get; set; }
    public List<SelectListItem>? PackagingTypes { get; set; }
    public List<SelectListItem>? PaymentMethods { get; set; }
}