using BL.Contract.IServices;
using BL.DTOs.UserSender;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/UserSender")]
public class UserSenderController : BaseController<IUserSenderService, TbUserSender, UserSenderDto, CreateUserSenderDto, UpdateUserSenderDto>
{
    public UserSenderController(IUserSenderService service) : base(service)
    {
    }
}
