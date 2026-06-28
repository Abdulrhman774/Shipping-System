using FluentValidation;
using BL.DTOs.Shippment;

namespace BL.Validators.Shippment;

public class UpdateShippmentDtoValidator : AbstractValidator<UpdateShippmentDto>
{
    public UpdateShippmentDtoValidator()
    {
        RuleFor(x => x.ShippingDate).NotEmpty().WithMessage("Shipping date is required.");
        RuleFor(x => x.SenderId).NotEmpty().WithMessage("Sender is required.");
        RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Receiver is required.");
        RuleFor(x => x.ShippingTypeId).NotEmpty().WithMessage("Shipping type is required.");
        
        RuleFor(x => x.Width).NotEmpty().GreaterThan(0).WithMessage("Width must be greater than 0.");
        RuleFor(x => x.Height).NotEmpty().GreaterThan(0).WithMessage("Height must be greater than 0.");
        RuleFor(x => x.Weight).NotEmpty().GreaterThan(0).WithMessage("Weight must be greater than 0.");
        RuleFor(x => x.Length).NotEmpty().GreaterThan(0).WithMessage("Length must be greater than 0.");
        
        RuleFor(x => x.PackageValue).NotEmpty().GreaterThan(0).WithMessage("Package value must be greater than 0.");
        RuleFor(x => x.ShippingRate).NotEmpty().GreaterThan(0).WithMessage("Shipping rate must be greater than 0.");
    }
}
