using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace UI.Models;

public class RegisterViewModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Second Name")]
    public string SecondName { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Third Name")]
    public string? ThirdName { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Url]
    [Display(Name = "Profile Image URL")]
    public string? ImageUrl { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [Display(Name = "Gender")]
    public enGender Gender { get; set; }
}