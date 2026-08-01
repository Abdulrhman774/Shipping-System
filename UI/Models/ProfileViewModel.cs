using BL.DTOs.User;
using BL.DTOs.Views;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class ProfileViewModel
    {
        public UserDto? User { get; set; }

        public List<ShipmentDetailsDto> ActiveShipments { get; set; } = new();

        public List<ShipmentDetailsDto> ShipmentHistory { get; set; } = new();

        public ShipmentDetailsDto? TrackingResult { get; set; }

        // ✅ خصائص الـ Password (مش موجودين قبل كده)
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [MinLength(9, ErrorMessage = "Password must be at least 9 characters")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}