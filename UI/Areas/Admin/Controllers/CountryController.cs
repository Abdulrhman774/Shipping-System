using BL.DTOs.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Controllers;
using UI.Services;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CountryController : BaseMvcController<CountryDto, CreateCountryDto, UpdateCountryDto>
    {
        public CountryController(GenericApiClient apiClient, ILogger<CountryController> logger)
            : base(apiClient, logger, "Api/Country")
        {
        }
    }
}
