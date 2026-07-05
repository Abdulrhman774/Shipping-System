using FluentValidation;
using BL.DTOs.ShippingPackaging;

namespace BL.Validators.ShippingPackaging;

public class UpdateShippingPackagingDtoValidator : AbstractValidator<UpdateShippingPackagingDto>
{
    public UpdateShippingPackagingDtoValidator()
    {
        RuleFor(x => x.ShippingPackagingAname)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ShippingPackagingAname));

        RuleFor(x => x.ShippingPackagingEname)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ShippingPackagingEname));
    }
}
