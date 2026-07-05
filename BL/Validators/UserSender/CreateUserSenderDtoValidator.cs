using FluentValidation;
using BL.DTOs.UserSender;

namespace BL.Validators.UserSender;

public class CreateUserSenderDtoValidator : AbstractValidator<CreateUserSenderDto>
{
    public CreateUserSenderDtoValidator()
    {
        RuleFor(x => x.SenderName).NotEmpty().WithMessage("Sender name is required.")
            .MaximumLength(200).WithMessage("Sender name must not exceed 200 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(200).WithMessage("Phone must not exceed 200 characters.");

        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(200).WithMessage("Postal code must not exceed 200 characters.");

        RuleFor(x => x.Contact)
            .NotEmpty().WithMessage("Contact is required.")
            .MaximumLength(200).WithMessage("Contact must not exceed 200 characters.");

        RuleFor(x => x.OtherAddress)
            .MaximumLength(500).WithMessage("Other address must not exceed 500 characters.");


        //RuleFor(x => x.UserId).NotEmpty().WithMessage("User is required.");
        RuleFor(x => x.CityId).NotEmpty().WithMessage("City is required.");
    }
}
