using FluentValidation;
using BL.DTOs.Auth;

namespace BL.DTOs.Auth
{
    // Declared here because the original file C:\Users\seif\source\repos\Shipping-System\BL\DTOs\Auth\Requests\RefreshRequestDto.cs
    // is entirely commented out, but we are instructed not to modify existing files and verify 0 build errors.
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = null!;
    }
}

namespace BL.Validators.Auth
{
    public class RefreshRequestDtoValidator : AbstractValidator<RefreshRequestDto>
    {
        public RefreshRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
