using BL.Contract.IServices;
using BL.DTOs.UserReceiver;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/UserReceiver")]
public class UserReceiverController : BaseController<IUserReceiverService, TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto>
{
    public UserReceiverController(IUserReceiverService service) : base(service)
    {
    }
}
