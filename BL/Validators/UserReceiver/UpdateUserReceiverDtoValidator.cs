using FluentValidation;
using BL.DTOs.UserReceiver;

namespace BL.Validators.UserReceiver;

public class UpdateUserReceiverDtoValidator : AbstractValidator<UpdateUserReceiverDto>
{
    public UpdateUserReceiverDtoValidator()
    {
        RuleFor(x => x.ReceiverName).NotEmpty().WithMessage("Receiver name is required.")
             .MaximumLength(200).WithMessage("Receiver name must not exceed 200 characters.")
             .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Receiver name must contain only letters and spaces.");

        RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.")
                .Matches(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
                .WithMessage("A valid email address is required.");

        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20)
                                        .Matches(@"^\+?[0-9]{7,15}$")
                                        .WithMessage("Phone number is invalid.");

        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(200).WithMessage("Postal code must not exceed 200 characters.");

        RuleFor(x => x.Contact)
            .NotEmpty().WithMessage("Contact is required.")
            .MaximumLength(200).WithMessage("Contact must not exceed 200 characters.")
            .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Contact must contain only letters and spaces.");


        RuleFor(x => x.OtherAddress)
            .MaximumLength(500).WithMessage("Other address must not exceed 500 characters.");


        RuleFor(x => x.CityId).NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.UserId)
            .Must(id => string.IsNullOrEmpty(id) || Guid.TryParse(id, out _))
            .WithMessage("Invalid user ID format.");

    }
}
