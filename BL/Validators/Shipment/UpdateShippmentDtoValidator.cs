using FluentValidation;
using BL.DTOs.Shipment;

namespace BL.Validators.Shipment;

public class UpdateShipmentDtoValidator : AbstractValidator<UpdateShipmentDto>
{
    public UpdateShipmentDtoValidator()
    {
        RuleFor(x => x.Width)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Width must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Width must not exceed 1000.");

        RuleFor(x => x.Height)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Height must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Height must not exceed 1000.");

        RuleFor(x => x.Weight)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Weight must not exceed 1000.");

        RuleFor(x => x.Length)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Length must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Length must not exceed 1000.");

        RuleFor(x => x.PackageValue)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("Package value must be greater than 0.")
            .LessThanOrEqualTo(1000000).WithMessage("Package value must not exceed 1000000.");


        RuleFor(x => x.ShippingDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Shipping date cannot be in the past.");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(x => x.ShippingDate)
            .When(x => x.DeliveryDate.HasValue)
            .WithMessage("Delivery date must be after shipping date.");


        RuleFor(x => x)
            .Must(x => x.SenderId != x.ReceiverId)
            .When(x => x.SenderId != Guid.Empty && x.ReceiverId != Guid.Empty)
            .WithMessage("Sender and receiver cannot be the same.");

        RuleFor(x => x.SenderId)
            .NotEmpty()
            .WithMessage("Sender is required.");

        RuleFor(x => x.ReceiverId)
            .NotEmpty()
            .WithMessage("Receiver is required.");

        RuleFor(x => x.ShippingTypeId)
            .NotEmpty()
            .WithMessage("Shipping type is required.");
    }
}