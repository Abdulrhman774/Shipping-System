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

namespace UI.Controllers;

[Authorize]
public class ShipmentController : Controller
{
    private readonly MvcShipmentService _shipmentService;
    private readonly ILogger<ShipmentController> _logger;

    public ShipmentController(MvcShipmentService shipmentService, ILogger<ShipmentController> logger)
    {
        _shipmentService = shipmentService;
        _logger = logger;
    }

    private Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("UserId"); 

        return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
    }

    private async Task<bool> PopulateDropdownDataAsync(CreateShipmentViewModel model)
    {
        var citiesResponse = await _shipmentService.GetCitiesAsync();
        var shippingTypesResponse = await _shipmentService.GetShippingTypesAsync();
        var packagingResponse = await _shipmentService.GetShippingPackagingAsync();
        var paymentMethodsResponse = await _shipmentService.GetPaymentMethodsAsync();

        if (!citiesResponse.Success || !shippingTypesResponse.Success ||
            !packagingResponse.Success || !paymentMethodsResponse.Success)
        {
            _logger.LogWarning("Failed to load one or more dropdown sources for CreateShipment.");
            return false;
        }

        model.Cities = citiesResponse.Data!
            .Select(c => new SelectListItem(c.CityAname ?? c.Id.ToString(), c.Id.ToString())).ToList();

        model.ShippingTypes = shippingTypesResponse.Data!
            .Select(s => new SelectListItem(s.ShippingTypeEname, s.Id.ToString())).ToList();

        model.PackagingTypes = packagingResponse.Data!
            .Select(p => new SelectListItem(p.ShippingPackagingEname, p.Id.ToString())).ToList();

        model.PaymentMethods = paymentMethodsResponse.Data!
            .Select(p => new SelectListItem(p.MethodEname, p.Id.ToString())).ToList();

        return true;
    }

    [HttpGet]
    public async Task<IActionResult> CreateShipment()
    {
        var model = new CreateShipmentViewModel();
        var success = await PopulateDropdownDataAsync(model);

        if (!success)
            return View("Error");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateShipment(CreateShipmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            if (!await PopulateDropdownDataAsync(model))
                return View("Error");

            return View(model);
        }

        var currentUserId = GetCurrentUserId();

        // 1) Resolve Sender
        Guid senderId;
        if (model.Sender.ExistingId.HasValue)
        {
            senderId = model.Sender.ExistingId.Value;
        }
        else
        {
            var senderResult = await _shipmentService.CreateSenderAsync(new CreateUserSenderDto
            {
                UserId = currentUserId,
                SenderName = model.Sender.Name,
                Email = model.Sender.Email,
                Phone = model.Sender.Phone,
                CityId = model.Sender.CityId,
                Address = model.Sender.Address,
                Contact = model.Sender.Contact!,
                OtherAddress = model.Sender.OtherAddress!,
                PostalCode = model.Sender.PostalCode
            });

            if (!senderResult.Success)
            {
                ModelState.AddModelError("", senderResult.Error ?? "Failed to save sender.");
                await PopulateDropdownDataAsync(model);
                return View(model);
            }

            senderId = senderResult.Data;
        }

        // 2) Resolve Receiver
        Guid receiverId;
        if (model.Receiver.ExistingId.HasValue)
        {
            receiverId = model.Receiver.ExistingId.Value;
        }
        else
        {
            var receiverResult = await _shipmentService.CreateReceiverAsync(new CreateUserReceiverDto
            {
                UserId = currentUserId,
                ReceiverName = model.Receiver.Name,
                Email = model.Receiver.Email,
                Phone = model.Receiver.Phone,
                CityId = model.Receiver.CityId,
                Address = model.Receiver.Address,
                Contact = model.Receiver.Contact!,
                OtherAddress = model.Receiver.OtherAddress!,
                PostalCode = model.Receiver.PostalCode
            });

            if (!receiverResult.Success)
            {
                ModelState.AddModelError("", receiverResult.Error ?? "Failed to save receiver.");
                await PopulateDropdownDataAsync(model);
                return View(model);
            }

            receiverId = receiverResult.Data;
        }

        // 3) Create the shipment itself
        var dto = new CreateShipmentDto
        {
            ShippingDate = model.ShippingDate,
            SenderId = senderId,
            ReceiverId = receiverId,
            ShippingTypeId = model.ShippingTypeId,
            ShippingPackagingId = model.ShippingPackagingId,
            Width = model.Width,
            Height = model.Height,
            Weight = model.Weight,
            Length = model.Length,
            PackageValue = model.PackageValue,
            PaymentMethodId = model.PaymentMethodId,
            UserSubscriptionId = model.UserSubscriptionId
        };

        var result = await _shipmentService.CreateShipmentAsync(dto);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Error ?? "Failed to create shipment.");
            await PopulateDropdownDataAsync(model);
            return View(model);
        }

        return RedirectToAction("Confirmation", new { id = result.Data });
    }

    [HttpGet]
    public IActionResult Confirmation(Guid id)
    {
        return View(id);
    }
}