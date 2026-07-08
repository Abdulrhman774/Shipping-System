using BL.Contract.IServices;
using BL.DTOs.Setting;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/Setting")]
public class SettingController : BaseController<ISettingService, TbSetting, SettingDto, CreateSettingDto, UpdateSettingDto>
{
    public SettingController(ISettingService service) : base(service)
    {
    }
}
