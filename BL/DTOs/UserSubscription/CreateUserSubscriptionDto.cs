namespace BL.DTOs.UserSubscription
{
    public class CreateUserSubscriptionDto
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
