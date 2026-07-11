using BL.DTOs.UserSubscription;
using FluentValidation;

namespace BL.Validators.UserSubscription;

public class UpdateUserSubscriptionDtoValidator : AbstractValidator<UpdateUserSubscriptionDto>
{
    public UpdateUserSubscriptionDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User is required.");
        RuleFor(x => x.PackageId).NotEmpty().WithMessage("Package is required.");
        RuleFor(x => x.SubscriptionDate).NotEmpty().WithMessage("Subscription date is required.");
    }
}
