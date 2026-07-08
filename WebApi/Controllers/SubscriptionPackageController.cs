using BL.Contract.IServices;
using BL.DTOs.SubscriptionPackage;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/SubscriptionPackage")]
public class SubscriptionPackageController : BaseController<ISubscriptionPackageService, TbSubscriptionPackage, SubscriptionPackageDto, CreateSubscriptionPackageDto, UpdateSubscriptionPackageDto>
{
    public SubscriptionPackageController(ISubscriptionPackageService service) : base(service)
    {
    }
}
