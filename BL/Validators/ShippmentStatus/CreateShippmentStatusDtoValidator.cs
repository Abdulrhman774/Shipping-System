using FluentValidation;
using BL.DTOs.ShippmentStatus;

namespace BL.Validators.ShippmentStatus;

public class CreateShippmentStatusDtoValidator : AbstractValidator<CreateShippmentStatusDto>
{
    public CreateShippmentStatusDtoValidator()
    {
        RuleFor(x => x.CarrierId).NotEmpty().WithMessage("Carrier is required.");
    }
}
