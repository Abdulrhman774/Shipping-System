using BL.Contract.IServices;
using BL.DTOs.UserSubscription;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/UserSubscription")]
public class UserSubscriptionController : BaseController<IUserSubscriptionService, TbUserSubscription, UserSubscriptionDto, CreateUserSubscriptionDto, UpdateUserSubscriptionDto>
{
    public UserSubscriptionController(IUserSubscriptionService service) : base(service)
    {
    }
}
