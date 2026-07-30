using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using BL.Validators.SharedCreateSenderReceiver;
using FluentValidation;

namespace BL.Validators.UserSender;

public class CreateUserSenderDtoValidator : SharedCreateSenderReceiverValidator<CreateUserSenderDto>
{
    public CreateUserSenderDtoValidator()
    {
       
    }
}
