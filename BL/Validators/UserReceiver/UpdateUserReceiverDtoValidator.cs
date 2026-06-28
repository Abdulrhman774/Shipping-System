using FluentValidation;
using BL.DTOs.UserReceiver;

namespace BL.Validators.UserReceiver;

public class UpdateUserReceiverDtoValidator : AbstractValidator<UpdateUserReceiverDto>
{
    public UpdateUserReceiverDtoValidator()
    {
        RuleFor(x => x.ReceiverName).NotEmpty().WithMessage("Receiver name is required.")
            .MaximumLength(200).WithMessage("Receiver name must not exceed 200 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(200).WithMessage("Phone must not exceed 200 characters.");

        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User is required.");
        RuleFor(x => x.CityId).NotEmpty().WithMessage("City is required.");
    }
}
