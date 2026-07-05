using FluentValidation;
using BL.DTOs.ShippingPackaging;

namespace BL.Validators.ShippingPackaging;

public class CreateShippingPackagingDtoValidator : AbstractValidator<CreateShippingPackagingDto>
{
    public CreateShippingPackagingDtoValidator()
    {
        RuleFor(x => x.ShippingPackagingAname)
            .NotEmpty()
            .WithMessage("إسم الشحن والتغليف مطلوب.")
            .MaximumLength(100);

        RuleFor(x => x.ShippingPackagingEname)
            .NotEmpty()
            .WithMessage("Shipping Packaging Name is required.")
            .MaximumLength(100);
    }
}
