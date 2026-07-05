using FluentValidation;
using BL.DTOs.ShipmentStatus;

namespace BL.Validators.ShipmentStatus;

public class CreateShipmentStatusDtoValidator : AbstractValidator<CreateShipmentStatusDto>
{
    public CreateShipmentStatusDtoValidator()
    {
        RuleFor(x => x.CarrierId).NotEmpty().WithMessage("Carrier is required.");
    }
}
