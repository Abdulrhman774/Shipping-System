using BL.DTOs.UserSender;
using BL.Validators.SharedCreateSenderReceiver;
using FluentValidation;

namespace BL.Validators.UserSender;

public class UpdateUserSenderDtoValidator : SharedUpdateSenderReceiverValidator<UpdateUserSenderDto>
{
    public UpdateUserSenderDtoValidator()
    {
        
    }
}
