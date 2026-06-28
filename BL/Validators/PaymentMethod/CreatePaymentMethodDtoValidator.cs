using FluentValidation;
using BL.DTOs.PaymentMethod;

namespace BL.Validators.PaymentMethod;

public class CreatePaymentMethodDtoValidator : AbstractValidator<CreatePaymentMethodDto>
{
    public CreatePaymentMethodDtoValidator()
    {
        RuleFor(x => x.MethdAname)
            .MaximumLength(200).WithMessage("اسم طريقة الدفع يجب أن لا يتجاوز 200 حرف.");

        RuleFor(x => x.MethodEname)
            .MaximumLength(200).WithMessage("Payment method name must not exceed 200 characters.");

        RuleFor(x => x.Commission)
            .GreaterThan(0).When(x => x.Commission.HasValue).WithMessage("Commission must be greater than 0.");
    }
}
