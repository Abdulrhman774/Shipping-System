using FluentValidation;
using BL.DTOs.UserSubscription;

namespace BL.Validators.UserSubscription;

public class CreateUserSubscriptionDtoValidator : AbstractValidator<CreateUserSubscriptionDto>
{
    public CreateUserSubscriptionDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User is required.");
        RuleFor(x => x.PackageId).NotEmpty().WithMessage("Package is required.");
        RuleFor(x => x.SubscriptionDate).NotEmpty().WithMessage("Subscription date is required.");
    }
}
