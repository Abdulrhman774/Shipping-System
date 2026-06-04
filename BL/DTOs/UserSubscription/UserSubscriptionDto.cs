using BL.DTOs.Base;

namespace BL.DTOs.UserSubscription
{
    public class UserSubscriptionDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
