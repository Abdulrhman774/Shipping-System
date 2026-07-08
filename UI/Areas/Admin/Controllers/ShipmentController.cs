using BL.Contract.IServices;
using BL.DTOs.Country;
using BL.DTOs.Shipment;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using UI.Controllers;
using UI.Services;

namespace UI.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize]
public class ShipmentController : BaseMvcController<CountryDto, CreateCountryDto, UpdateCountryDto>
{
    public ShipmentController(GenericApiClient apiClient, ILogger<ShipmentController> logger)
        : base(apiClient, logger, "Api/Shipment")
    {
    }
}
