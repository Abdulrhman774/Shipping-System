using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using BL.Validators.SharedCreateSenderReceiver;
using FluentValidation;

namespace BL.Validators.UserReceiver;

public class UpdateUserReceiverDtoValidator : SharedUpdateSenderReceiverValidator<UpdateUserReceiverDto>
{
    public UpdateUserReceiverDtoValidator()
    {
       
    }
}
