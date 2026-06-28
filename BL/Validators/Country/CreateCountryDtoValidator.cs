using FluentValidation;
using BL.DTOs.Country;

namespace BL.Validators.Country;

public class CreateCountryDtoValidator : AbstractValidator<CreateCountryDto>
{
    public CreateCountryDtoValidator()
    {
        RuleFor(x => x.CountryAname)
            .MaximumLength(200).WithMessage("اسم الدولة يجب أن لا يتجاوز 200 حرف.");

        RuleFor(x => x.CountryEname)
            .MaximumLength(200).WithMessage("Country name must not exceed 200 characters.");
    }
}
