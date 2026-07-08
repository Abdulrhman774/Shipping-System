using BL.Contract.IServices;
using BL.DTOs.Country;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("Api/Country")]
public class CountryController : BaseController<ICountryService, TbCountry, CountryDto, CreateCountryDto, UpdateCountryDto>
{
    public CountryController(ICountryService service) : base(service)
    {
    }
}
