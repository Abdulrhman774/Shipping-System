namespace BL.DTOs.UserSender
{
    public class UpdateUserSenderDto
    {
        public Guid UserId { get; set; }
        public string SenderName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid CityId { get; set; }
        public string Address { get; set; } = null!;
    }
}
