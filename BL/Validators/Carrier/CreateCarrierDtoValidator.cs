using FluentValidation;
using BL.DTOs.Carrier;

namespace BL.Validators.Carrier;

public class CreateCarrierDtoValidator : AbstractValidator<CreateCarrierDto>
{
    public CreateCarrierDtoValidator()
    {
        RuleFor(x => x.CarrierName)
            .NotEmpty().WithMessage("Carrier name is required.")
            .MaximumLength(200).WithMessage("Carrier name must not exceed 200 characters.");
    }
}
