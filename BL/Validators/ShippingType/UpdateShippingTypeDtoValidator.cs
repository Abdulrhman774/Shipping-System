using FluentValidation;
using BL.DTOs.ShippingType;

namespace BL.Validators.ShippingType;

public class UpdateShippingTypeDtoValidator : AbstractValidator<UpdateShippingTypeDto>
{
    public UpdateShippingTypeDtoValidator()
    {
        RuleFor(x => x.ShippingTypeAname)
            .MaximumLength(200).WithMessage("اسم نوع الشحن يجب أن لا يتجاوز 200 حرف.");

        RuleFor(x => x.ShippingTypeEname)
            .MaximumLength(200).WithMessage("Shipping type name must not exceed 200 characters.");

        RuleFor(x => x.ShippingFactor)
            .NotEmpty().WithMessage("Shipping factor is required.")
            .GreaterThan(0).WithMessage("Shipping factor must be greater than 0.");
    }
}
