using FluentValidation;
using BL.DTOs.Setting;

namespace BL.Validators.Setting;

public class UpdateSettingDtoValidator : AbstractValidator<UpdateSettingDto>
{
    public UpdateSettingDtoValidator()
    {
        RuleFor(x => x.KiloMeterRate)
            .GreaterThan(0).When(x => x.KiloMeterRate.HasValue).WithMessage("Kilo meter rate must be greater than 0.");

        RuleFor(x => x.KilooGramRate)
            .GreaterThan(0).When(x => x.KilooGramRate.HasValue).WithMessage("Kilogram rate must be greater than 0.");
    }
}
