using FluentValidation;
using BL.DTOs.City;
using System;

namespace BL.Validators.City;

public class UpdateCityDtoValidator : AbstractValidator<UpdateCityDto>
{
    public UpdateCityDtoValidator()
    {
        RuleFor(x => x.CityAname)
            .MaximumLength(10).WithMessage("اسم المدينة يجب أن لا يتجاوز 10 أحرف.");

        RuleFor(x => x.CityEname)
            .MaximumLength(10).WithMessage("City name must not exceed 10 characters.");

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage("Country is required.")
            .Must(id => id != Guid.Empty).WithMessage("Country is required.");
    }
}
