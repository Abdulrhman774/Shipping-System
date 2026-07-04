using FluentValidation;
using BL.DTOs.SubscriptionPackage;

namespace BL.Validators.SubscriptionPackage;

public class CreateSubscriptionPackageDtoValidator : AbstractValidator<CreateSubscriptionPackageDto>
{
    public CreateSubscriptionPackageDtoValidator()
    {
        RuleFor(x => x.PackageName).NotEmpty().WithMessage("Package name is required.")
            .MaximumLength(200).WithMessage("Package name must not exceed 200 characters.");

        RuleFor(x => x.ShippimentCount).NotEmpty().GreaterThan(0).WithMessage("Shipment count must be greater than 0.");
        RuleFor(x => x.NumberOfKiloMeters).NotEmpty().GreaterThan(0).WithMessage("Number of kilometers must be greater than 0.");
        RuleFor(x => x.TotalWeight).NotEmpty().GreaterThan(0).WithMessage("Total weight must be greater than 0.");
    }
}
