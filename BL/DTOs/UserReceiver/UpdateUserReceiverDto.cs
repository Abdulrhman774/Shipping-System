namespace BL.DTOs.UserReceiver
{
    public class UpdateUserReceiverDto
    {
        public Guid UserId { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid CityId { get; set; }
        public string Address { get; set; } = null!;
    }
}
