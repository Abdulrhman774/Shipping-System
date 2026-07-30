using BL.Common;
using BL.DTOs.Shipment;
using BL.DTOs.UserSender;
using BL.DTOs.UserReceiver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using UI.Models.Shipment;
using UI.Services;
using BL.Contract.IServices.Shipment;
using BL.Contract.IServices;

namespace UI.Controllers;

[Authorize]
public class ShipmentController : Controller
{
    private readonly MvcShipmentService _shipmentServiceApi;
    private readonly IShipmentService _shipmentService;
    private readonly ICityService _cityService;
    private readonly IShippingTypeService _shippingTypeService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ICountryService _countryService;
    private readonly IShippingPackagingService _shippingPackagingService;
    private readonly IUserSenderService _userSenderService;
    private readonly IUserReceiverService _userReceiverService;
    private readonly ILogger<ShipmentController> _logger;

    public ShipmentController(MvcShipmentService shipmentServiceApi, ILogger<ShipmentController> logger
        , IShipmentService shipmentService, ICountryService countryService, ICityService cityService
        , IShippingTypeService shippingTypeService, IPaymentMethodService paymentMethodService, 
        IShippingPackagingService shippingPackagingService, IUserSenderService userSenderService, IUserReceiverService userReceiverService)
    {
        _shipmentServiceApi = shipmentServiceApi;
        _shipmentService = shipmentService;
        _logger = logger;
        _countryService = countryService;
        _cityService = cityService;
        _shippingTypeService = shippingTypeService;
        _paymentMethodService = paymentMethodService;
        _shippingPackagingService = shippingPackagingService;
        _userSenderService = userSenderService;
        _userReceiverService = userReceiverService;
    }

    private string GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("UserId");

        return userIdString ?? string.Empty;
    }

    private async Task<bool> PopulateDropdownDataAsync(ShipmentWizardViewModel model)
    {
        var citiesResponse = await _cityService.GetAllAsync();
        var countriesResponse = await _countryService.GetAllAsync();
        var shippingTypesResponse = await _shippingTypeService.GetAllAsync();
        var packagingResponse = await _shippingPackagingService.GetAllAsync();
        var paymentMethodsResponse = await _paymentMethodService.GetAllAsync();

        if (citiesResponse.IsFailure || shippingTypesResponse.IsFailure ||
            packagingResponse.IsFailure || paymentMethodsResponse.IsFailure)
        {
            _logger.LogWarning("Failed to load one or more dropdown sources for CreateShipment.");
            return false;
        }

        model.Cities = citiesResponse.Value!
            .Select(c => new SelectListItem(c.CityAname ?? c.Id.ToString(), c.Id.ToString())).ToList();

        model.Shipment.ShippingTypes = shippingTypesResponse.Value!
            .Select(s => new SelectListItem(s.ShippingTypeEname, s.Id.ToString())).ToList();

        model.Shipment.PackagingTypes = packagingResponse.Value!
            .Select(p => new SelectListItem(p.ShippingPackagingEname, p.Id.ToString())).ToList();

        model.Shipment.PaymentMethods = paymentMethodsResponse.Value!
            .Select(p => new SelectListItem(p.MethodEname, p.Id.ToString())).ToList();

        return true;
    }

    [HttpGet]
    public async Task<IActionResult> CreateShipment()
    {
        var model = new ShipmentWizardViewModel();
        var success = await PopulateDropdownDataAsync(model);

        if (!success)
            return View("Error");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateShipment(ShipmentWizardViewModel model)
    {
        if (!ModelState.IsValid)
        {
            if (!await PopulateDropdownDataAsync(model))
                return View("Error");

            return View(model);
        }

        var currentUserId = GetCurrentUserId();

        var dto = new CreateShipmentRequestDto
        {
            SenderDto = new CreateUserSenderDto
            {
                UserId = currentUserId,
                Name = model.Sender.SenderName,
                Email = model.Sender.Email,
                Phone = model.Sender.Phone,
                CityId = model.Sender.CityId,
                Address = model.Sender.Address,
                Contact = model.Sender.Contact!,
                OtherAddress = model.Sender.OtherAddress!,
                PostalCode = model.Sender.PostalCode,
                IsDefaultAddress = model.Sender.IsDefaultAddress
            },
            ReceiverDto = new CreateUserReceiverDto
            {
                UserId = currentUserId,
                Name = model.Receiver.ReceiverName,
                Email = model.Receiver.Email,
                Phone = model.Receiver.Phone,
                CityId = model.Receiver.CityId,
                Address = model.Receiver.Address,
                Contact = model.Receiver.Contact!,
                OtherAddress = model.Receiver.OtherAddress!,
                PostalCode = model.Receiver.PostalCode,
                IsDefaultAddress = model.Receiver.IsDefaultAddress
            },
            ShipmentDto = new CreateShipmentDto
            {
                ShippingDate = model.Shipment.ShippingDate,
                DeliveryDate = model.Shipment.DeliveryDate,
                ShippingTypeId = model.Shipment.ShippingTypeId,
                ShippingPackagingId = model.Shipment.ShippingPackagingId,
                Width = model.Shipment.Width,
                Height = model.Shipment.Height,
                Weight = model.Shipment.Weight,
                Length = model.Shipment.Length,
                PackageValue = model.Shipment.PackageValue,
                PaymentMethodId = model.Shipment.PaymentMethodId,
                UserSubscriptionId = model.Shipment.UserSubscriptionId
            }
        };

        var result = await _shipmentService.CreateShipment(dto);

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.FirstError?.Code ?? "Failed to create shipment. Please try again.";
            ModelState.AddModelError("", result.FirstError?.Code ?? "Failed to create shipment.");
            await PopulateDropdownDataAsync(model);
            return View(model);
        }

        TempData["SuccessMessage"] = "Shipment created successfully!";
        return RedirectToAction("Confirmation", new { id = result.Value.Id });
    }

    [HttpGet]
    public IActionResult Confirmation(Guid id)
    {
        return View(id);
    }
}