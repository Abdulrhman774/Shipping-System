using BL.Contract.IServices;
using BL.DTOs.City;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/City")]
public class CityController : BaseController<ICityService, TbCity, CityDto, CreateCityDto, UpdateCityDto>
{
    public CityController(ICityService service) : base(service)
    {
    }
}
