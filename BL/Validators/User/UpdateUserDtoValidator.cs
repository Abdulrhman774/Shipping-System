using BL.DTOs.User;
using FluentValidation;
using System;

namespace BL.Validators.User
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            // الاسم الأول: إلزامي، الحد الأقصى 100 حرف، وحروف فقط (أحرف ومسافات)
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("First name must contain only letters and spaces.");

            // الاسم الثاني: إلزامي، نفس القيود
            RuleFor(x => x.SecondName)
                .NotEmpty().WithMessage("Second name is required.")
                .MaximumLength(100).WithMessage("Second name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Second name must contain only letters and spaces.");

            // الاسم الثالث: اختياري، لكن إذا وُجد لا يتجاوز 100 حرف
            RuleFor(x => x.ThirdName)
                .MaximumLength(100).WithMessage("Third name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s]*$").WithMessage("Third name must contain only letters and spaces.") // يسمح بفراغ
                .When(x => !string.IsNullOrWhiteSpace(x.ThirdName));

            // الاسم الأخير: إلزامي
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z\u0600-\u06FF\s]+$").WithMessage("Last name must contain only letters and spaces.");

            // تاريخ الميلاد: إلزامي، يجب أن يكون في الماضي
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date of birth must be in the past.");

            // الجنس: إلزامي، يجب أن يكون قيمة صحيحة من الـ enum
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .IsInEnum().WithMessage("Gender must be a valid option.");

            // البريد الإلكتروني: إلزامي، صيغة صحيحة، حد أقصى
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

            // رقم الهاتف: اختياري، لكن إذا وُجد يجب أن يكون صيغة صحيحة
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{7,15}$")
                .WithMessage("Phone number must be a valid format (e.g. +201234567890).")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            // رابط الصورة: اختياري، إذا وُجد يجب أن يكون رابطاً صحيحاً
            RuleFor(x => x.ImageUrl)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Image URL must be a valid URL.")
                .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));
        }
    }
}