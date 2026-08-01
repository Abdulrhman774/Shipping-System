using BL.Common;
using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.Contract.IvwServices;
using BL.DTOs.Shipment;
using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using BL.DTOs.Views;
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
    #region Fields
    private readonly IShipmentViewService _shipmentViewService;
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
    #endregion

    public ShipmentController(
        MvcShipmentService shipmentServiceApi,
        ILogger<ShipmentController> logger,
        IShipmentService shipmentService,
        ICountryService countryService,
        ICityService cityService,
        IShippingTypeService shippingTypeService,
        IPaymentMethodService paymentMethodService,
        IShippingPackagingService shippingPackagingService,
        IUserSenderService userSenderService,
        IUserReceiverService userReceiverService,
        IShipmentViewService shipmentViewService)
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
        _shipmentViewService = shipmentViewService;
    }

    #region Private Methods
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
    #endregion

    // ================================================================
    // ✅ GET: /Shipment/Create
    // ================================================================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new ShipmentWizardViewModel();
        var success = await PopulateDropdownDataAsync(model);

        if (!success)
            return View("Error");

        return View("UpsertShipment", model);
    }

    // ================================================================
    // ✅ POST: /Shipment/Create
    // ================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShipmentWizardViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownDataAsync(model);
            return View("UpsertShipment", model);
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
                Contact = model.Sender.Contact ?? string.Empty,
                OtherAddress = model.Sender.OtherAddress ?? string.Empty,
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
                Contact = model.Receiver.Contact ?? string.Empty,
                OtherAddress = model.Receiver.OtherAddress ?? string.Empty,
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
            return View("UpsertShipment", model);
        }

        TempData["SuccessMessage"] = "Shipment created successfully!";
        return RedirectToAction("Confirmation", new { id = result.Value.Id });
    }

    // ================================================================
    // ✅ GET: /Shipment/Update/{id}
    // ================================================================
    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Update(Guid id)
    {
        var model = new ShipmentWizardViewModel();
        var success = await PopulateDropdownDataAsync(model);

        if (!success)
            return View("Error");

        var result = await _shipmentViewService.GetShipmentDetailByIdAsync(id);

        if (result.IsSuccess && result.Value != null)
        {
            var shipment = result.Value;
            model.ShipmentId = shipment.ShipmentId;
            model.SenderId = shipment.SenderId;
            model.ReceiverId = shipment.ReceiverId;

            // ملء بيانات المرسل (Sender)
            model.Sender.SenderName = shipment.SenderName ?? string.Empty;
            model.Sender.Email = shipment.SenderEmail ?? string.Empty;
            model.Sender.Phone = shipment.SenderPhone ?? string.Empty;
            model.Sender.Address = shipment.SenderAddress ?? string.Empty;
            model.Sender.PostalCode = shipment.SenderPostalCode ?? string.Empty;
            model.Sender.Contact = shipment.SenderContact ?? string.Empty;
            model.Sender.OtherAddress = shipment.SenderOtherAddress ?? string.Empty;
            model.Sender.CityId = shipment.SenderCityId ?? Guid.Empty;
            model.Sender.IsDefaultAddress = shipment.SenderIsDefault ?? false;

            // ملء بيانات المستلم (Receiver)
            model.Receiver.ReceiverName = shipment.ReceiverName ?? string.Empty;
            model.Receiver.Email = shipment.ReceiverEmail ?? string.Empty;
            model.Receiver.Phone = shipment.ReceiverPhone ?? string.Empty;
            model.Receiver.Address = shipment.ReceiverAddress ?? string.Empty;
            model.Receiver.PostalCode = shipment.ReceiverPostalCode ?? string.Empty;
            model.Receiver.Contact = shipment.ReceiverContact ?? string.Empty;
            model.Receiver.OtherAddress = shipment.ReceiverOtherAddress ?? string.Empty;
            model.Receiver.CityId = shipment.ReceiverCityId ?? Guid.Empty;
            model.Receiver.IsDefaultAddress = shipment.ReceiverIsDefault ?? false;

            // ملء بيانات الشحنة
            model.Shipment.ShippingDate = shipment.ShippingDate;
            model.Shipment.DeliveryDate = shipment.DeliveryDate;
            model.Shipment.ShippingTypeId = shipment.ShippingTypeId;
            model.Shipment.ShippingPackagingId = shipment.ShippingPackagingId;
            model.Shipment.Weight = shipment.Weight;
            model.Shipment.Width = shipment.Width;
            model.Shipment.Height = shipment.Height;
            model.Shipment.Length = shipment.Length;
            model.Shipment.PackageValue = shipment.PackageValue;
            model.Shipment.PaymentMethodId = shipment.PaymentMethodId;
            model.Shipment.UserSubscriptionId = shipment.UserSubscriptionId;
        }
        else
        {
            TempData["ErrorMessage"] = "Shipment not found.";
            return RedirectToAction("History");
        }

        return View("UpsertShipment", model);
    }

    // ================================================================
    // ✅ POST: /Shipment/Update
    // ================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ShipmentWizardViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownDataAsync(model);

            // جمع كل رسائل الخطأ
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(m => !string.IsNullOrEmpty(m))
                .ToArray();

            TempData["ValidationErrors"] = errors;

            TempData["ErrorMessage"] = "Please correct the errors in the form.";

            return RedirectToAction("Update", new { id = model.ShipmentId });
        }

        if (model.SenderId == Guid.Empty || model.ReceiverId == Guid.Empty)
        {
            TempData["ErrorMessage"] = "Sender or Receiver ID is missing.";
            await PopulateDropdownDataAsync(model);
            return RedirectToAction("Update", new { id = model.ShipmentId });
        }

        try
        {
            var requestDto = new UpdateShipmentRequestDto
            {
                ShipmentDto = new UpdateShipmentDto
                {
                    SenderId = model.SenderId,
                    ReceiverId = model.ReceiverId,
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
                    UserSubscriptionId = model.Shipment.UserSubscriptionId,
                    ReferenceId = model.Shipment.ReferenceId
                },
                SenderDto = new UpdateUserSenderDto
                {
                    Name = model.Sender.SenderName,
                    Email = model.Sender.Email,
                    Phone = model.Sender.Phone,
                    Address = model.Sender.Address,
                    PostalCode = model.Sender.PostalCode,
                    Contact = model.Sender.Contact ?? string.Empty,
                    OtherAddress = model.Sender.OtherAddress ?? string.Empty,
                    CityId = model.Sender.CityId,
                    IsDefaultAddress = model.Sender.IsDefaultAddress
                },
                ReceiverDto = new UpdateUserReceiverDto
                {
                    Name = model.Receiver.ReceiverName,
                    Email = model.Receiver.Email,
                    Phone = model.Receiver.Phone,
                    Address = model.Receiver.Address,
                    PostalCode = model.Receiver.PostalCode,
                    Contact = model.Receiver.Contact ?? string.Empty,
                    OtherAddress = model.Receiver.OtherAddress ?? string.Empty,
                    CityId = model.Receiver.CityId,
                    IsDefaultAddress = model.Receiver.IsDefaultAddress
                }
            };

            var result = await _shipmentService.UpdateShipment(model.ShipmentId!.Value, requestDto);

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.FirstError?.Description ?? "Failed to update shipment.";
                await PopulateDropdownDataAsync(model);
                return RedirectToAction("Update", new { id = model.ShipmentId });
            }

            TempData["SuccessMessage"] = "Shipment updated successfully!";
            return RedirectToAction("History");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating shipment {Id}", model.ShipmentId);
            TempData["ErrorMessage"] = "An error occurred while updating the shipment.";
            await PopulateDropdownDataAsync(model);
            return RedirectToAction("Update", new { id = model.ShipmentId });
        }
    }

    // ================================================================
    // ✅ GET: /Shipment/Confirmation/{id}
    // ================================================================
    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Confirmation(Guid id)
    {
        // تحقق من وجود رسالة نجاح في TempData
        var successMessage = TempData["SuccessMessage"] as string;

        // إذا لم تكن هناك رسالة (أي أن المستخدم وصل للصفحة مباشرة أو بعد Refresh)
        if (string.IsNullOrEmpty(successMessage))
        {
            // إعادة توجيه إلى History لمنع إعادة الإرسال
            return RedirectToAction("History");
        }

        // تمرير الرسالة إلى الـ View (اختياري)
        ViewBag.SuccessMessage = successMessage;

        return View(id);
    }

    // ================================================================
    // ✅ GET: /Shipment/History
    // ================================================================
    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> History()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var result = await _shipmentViewService.GetShipmentsByUserAsync(Guid.Parse(userId));

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.FirstError?.Description ?? "Failed to load shipment history.";
            return View(new List<ShipmentDetailsDto>());
        }

        var shipments = result.Value ?? new List<ShipmentDetailsDto>();
        _logger.LogInformation($"Found {shipments.Count()} shipments for user {userId}");

        return View(shipments);
    }

    // ================================================================
    // ✅ GET: /Shipment/Details/{id}
    // ================================================================
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _shipmentViewService.GetShipmentDetailByIdAsync(id);

        if (result.IsFailure || result.Value == null)
        {
            TempData["ErrorMessage"] = result.FirstError?.Description ?? "Shipment not found.";
            return RedirectToAction("History");
        }

        return View(result.Value);
    }
}