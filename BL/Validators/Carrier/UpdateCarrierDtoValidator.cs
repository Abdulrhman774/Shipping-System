using FluentValidation;
using BL.DTOs.Carrier;

namespace BL.Validators.Carrier;

public class UpdateCarrierDtoValidator : AbstractValidator<UpdateCarrierDto>
{
    public UpdateCarrierDtoValidator()
    {
        RuleFor(x => x.CarrierName)
            .NotEmpty().WithMessage("Carrier name is required.")
            .MaximumLength(200).WithMessage("Carrier name must not exceed 200 characters.");
    }
}
