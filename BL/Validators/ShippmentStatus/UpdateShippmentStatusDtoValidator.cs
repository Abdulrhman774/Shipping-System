using FluentValidation;
using BL.DTOs.ShippmentStatus;

namespace BL.Validators.ShippmentStatus;

public class UpdateShippmentStatusDtoValidator : AbstractValidator<UpdateShippmentStatusDto>
{
    public UpdateShippmentStatusDtoValidator()
    {
        RuleFor(x => x.CarrierId).NotEmpty().WithMessage("Carrier is required.");
    }
}
