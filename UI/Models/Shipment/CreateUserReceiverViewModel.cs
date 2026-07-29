using System.ComponentModel.DataAnnotations;

namespace UI.Models.Shipment;

public class CreateUserReceiverViewModel
{
    [Required(ErrorMessage = "Receiver name is required.")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    [Display(Name = "Company or Name")]
    public string ReceiverName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Telephone")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postal code is required.")]
    [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters.")]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Contact")]
    public string Contact { get; set; } = null!;

    [Required(ErrorMessage = "Please select a city.")]
    [Display(Name = "City")]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
    [Display(Name = "Address")]
    public string Address { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Other Address Info")]
    public string? OtherAddress { get; set; }

    [Display(Name = "Use as default address")]
    public bool IsDefaultAddress { get; set; }
}