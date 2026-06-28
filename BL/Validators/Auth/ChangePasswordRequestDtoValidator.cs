using FluentValidation;
using BL.DTOs.Auth.Requests;
using System;

namespace BL.Validators.Auth;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequstDto>
{
    public ChangePasswordRequestDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("User ID must be valid.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.")
            .MinimumLength(9).WithMessage("Password must be at least 9 characters.")
            .Matches("[A-Z]").WithMessage("Must contain uppercase.")
            .Matches("[a-z]").WithMessage("Must contain lowercase.")
            .Matches("[0-9]").WithMessage("Must contain digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Must contain special character.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(9).WithMessage("Password must be at least 9 characters.")
            .Matches("[A-Z]").WithMessage("Must contain uppercase.")
            .Matches("[a-z]").WithMessage("Must contain lowercase.")
            .Matches("[0-9]").WithMessage("Must contain digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Must contain special character.");
    }
}
