using BL.DTOs.UserReceiver;
using BL.Validators.SharedCreateSenderReceiver;
using FluentValidation;

namespace BL.Validators.UserReceiver;

public class CreateUserReceiverDtoValidator : SharedCreateSenderReceiverValidator<CreateUserReceiverDto>
{
    public CreateUserReceiverDtoValidator()
    {
        
    }
}
