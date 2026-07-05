using FluentValidation;
using BL.DTOs.ShipmentStatus;

namespace BL.Validators.ShipmentStatus;

public class UpdateShipmentStatusDtoValidator : AbstractValidator<UpdateShipmentStatusDto>
{
    public UpdateShipmentStatusDtoValidator()
    {
        RuleFor(x => x.CarrierId).NotEmpty().WithMessage("Carrier is required.");
    }
}
