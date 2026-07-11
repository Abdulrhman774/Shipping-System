using FluentValidation;
using BL.DTOs.City;
using System;

namespace BL.Validators.City;

public class CreateCityDtoValidator : AbstractValidator<CreateCityDto>
{
    public CreateCityDtoValidator()
    {
        RuleFor(x => x.CityAname)
            .MaximumLength(100).WithMessage("اسم المدينة يجب أن لا يتجاوز 100 حرف.");

        RuleFor(x => x.CityEname)
            .MaximumLength(100).WithMessage("City name must not exceed 100 characters.");

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage("Country is required.")
            .Must(id => id != Guid.Empty).WithMessage("Country is required.");
    }
}
