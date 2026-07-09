using System.ComponentModel.DataAnnotations;

namespace UI.Models.Shipment;


public class CreateUserReceiverViewModel
{
    [Required]
    public string ReceiverName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    public string? Contact { get; set; }

    [Required]
    public Guid CityId { get; set; }

    [Required]
    public string Address { get; set; } = string.Empty;

    public string? OtherAddress { get; set; }

    public bool IsDefaultAddress { get; set; }
}