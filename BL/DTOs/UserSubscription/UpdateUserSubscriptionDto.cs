namespace BL.DTOs.UserSubscription
{
    public class UpdateUserSubscriptionDto
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
