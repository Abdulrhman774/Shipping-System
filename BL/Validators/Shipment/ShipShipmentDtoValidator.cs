using BL.DTOs.Shipment;
using FluentValidation;

namespace BL.Validators.Shipment;

public class ShipShipmentDtoValidator : AbstractValidator<ShipShipmentDto>
{
    public ShipShipmentDtoValidator()
    {
        RuleFor(x => x.CarrierId)
            .NotEmpty().WithMessage("Carrier is required when shipping a shipment.")
            .Must(id => id != Guid.Empty).WithMessage("A valid Carrier ID must be provided.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters.")
            .When(x => x.Note is not null);
    }
}
