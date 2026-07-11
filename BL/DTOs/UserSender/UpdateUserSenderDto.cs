namespace BL.DTOs.UserSender
{
    public class UpdateUserSenderDto
    {
        public string? UserId { get; set; }
        public string SenderName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string OtherAddress { get; set; } = null!;
        public bool IsDefaultAddress { get; set; }
        public Guid CityId { get; set; }
        public string Address { get; set; } = null!;
    }
}
