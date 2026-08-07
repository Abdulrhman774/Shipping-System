using BL.Common.Results;
using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.Contract.IvwServices;
using BL.DTOs.Shipment;
using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Areas.Admin.Models;
using UI.Helpers;

namespace UI.Areas.Admin.Controllers;

public class ShipmentController : BaseAdminController
{
    private readonly IShipmentService _shipmentService;
    private readonly IShipmentViewService _shipmentViewService;
    private readonly ICarrierService _carrierService;
    private readonly IShippingTypeService _shippingTypeService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ICityService _cityService;
    private readonly IShippingPackagingService _packagingService;

    public ShipmentController(
        IShipmentService shipmentService,
        IShipmentViewService shipmentViewService,
        ICarrierService carrierService,
        IShippingTypeService shippingTypeService,
        IPaymentMethodService paymentMethodService,
        ICityService cityService,
        IShippingPackagingService packagingService)
    {
        _shipmentService = shipmentService;
        _shipmentViewService = shipmentViewService;
        _carrierService = carrierService;
        _shippingTypeService = shippingTypeService;
        _paymentMethodService = paymentMethodService;
        _cityService = cityService;
        _packagingService = packagingService;
    }

    // ================================================================
    // INDEX  — all 4 admin roles
    // ================================================================
    public async Task<IActionResult> Index(
        string? search = null,
        string? status = null,
        int page = 1,
        int pageSize = 10)
    {
        var result = await _shipmentViewService.GetShipmentDetailsPagedAsync(page, pageSize);

        if (result.IsFailure || result.Value is null)
            return View(new ShipmentIndexViewModel());

        var paged = result.Value;

        // Optional client-side search filter (tracking number or sender name)
        var items = paged.Items.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
            items = items.Where(s =>
                (s.TrackingNumber ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (s.SenderName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (s.ReceiverName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(status) && status != "All" &&
            Enum.TryParse<enShipmentStatus>(status, out var statusFilter))
            items = items.Where(s => s.Status == statusFilter);

        var rowItems = items.Select((s, i) => new ShipmentRowItem
        {
            RowNumber = (page - 1) * pageSize + i + 1,
            ShipmentId = s.ShipmentId,
            TrackingNumber = s.TrackingNumber ?? "—",
            SenderName = s.SenderName ?? "—",
            ReceiverName = s.ReceiverName ?? "—",
            ShippingDate = s.ShippingDate.ToString("dd MMM yyyy"),
            Status = s.Status,
        }).ToList();

        var model = new ShipmentIndexViewModel
        {
            Shipments = rowItems,
            Pagination = new PaginationModel
            {
                CurrentPage = paged.PageNumber,
                TotalPages = paged.TotalPages,
                TotalItems = paged.TotalCount,
                PageSize = paged.PageSize,
                ItemLabel = "shipments"
            }
        };

        ViewBag.Search = search;
        ViewBag.Status = status ?? "All";

        return View(model);
    }

    // ================================================================
    // DETAILS  — all 4 admin roles
    // ================================================================
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _shipmentViewService.GetShipmentDetailByIdAsync(id);

        if (result.IsFailure || result.Value is null)
        {
            TempData["ErrorMessage"] = "Shipment not found.";
            return RedirectToAction(nameof(Index));
        }

        // تحميل شركات النقل للـ dropdown في صفحة التفاصيل
        var carriers = await LoadCarriersAsync();
        ViewBag.Carriers = carriers;

        return View(result.Value);
    }
    // ================================================================
    // CREATE  — Admin, Op
    // ================================================================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new ShipmentFormViewModel();
        await PopulateDropdownsAsync(vm);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShipmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        var requestDto = new CreateShipmentRequestDto
        {
            SenderDto = new CreateUserSenderDto
            {
                Name = model.SenderName,
                Email = model.SenderEmail,
                Phone = model.SenderPhone,
                PostalCode = model.SenderPostalCode,
                Contact = model.SenderContact,
                Address = model.SenderAddress,
                OtherAddress = model.SenderOtherAddress,
                CityId = model.SenderCityId,
                IsDefaultAddress = model.SenderIsDefault,
            },
            ReceiverDto = new CreateUserReceiverDto
            {
                Name = model.ReceiverName,
                Email = model.ReceiverEmail,
                Phone = model.ReceiverPhone,
                PostalCode = model.ReceiverPostalCode,
                Contact = model.ReceiverContact,
                Address = model.ReceiverAddress,
                OtherAddress = model.ReceiverOtherAddress,
                CityId = model.ReceiverCityId,
                IsDefaultAddress = model.ReceiverIsDefault,
            },
            ShipmentDto = new CreateShipmentDto
            {
                ShippingDate = model.ShippingDate,
                DeliveryDate = model.DeliveryDate,
                ShippingTypeId = model.ShippingTypeId,
                ShippingPackagingId = model.ShippingPackagingId,
                Width = model.Width,
                Height = model.Height,
                Weight = model.Weight,
                Length = model.Length,
                PackageValue = model.PackageValue,
                PaymentMethodId = model.PaymentMethodId,
                UserSubscriptionId = model.UserSubscriptionId,
            }
        };

        var result = await _shipmentService.CreateShipment(requestDto);

        if (result.IsFailure)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        TempData["SuccessMessage"] = $"Shipment created successfully. Tracking: {result.Value.TrackingNumber}";
        return RedirectToAction(nameof(Details), new { id = result.Value.Id });
    }

    // ================================================================
    // EDIT  — Admin, Op
    // ================================================================
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _shipmentViewService.GetShipmentDetailByIdAsync(id);

        if (result.IsFailure || result.Value is null)
        {
            TempData["ErrorMessage"] = "Shipment not found.";
            return RedirectToAction(nameof(Index));
        }

        var s = result.Value;
        var vm = new ShipmentFormViewModel
        {
            ShipmentId = s.ShipmentId,
            // ✅ تم إضافة SenderId و ReceiverId
            SenderId = s.SenderId,
            ReceiverId = s.ReceiverId,
            ShippingDate = s.ShippingDate,
            DeliveryDate = s.DeliveryDate,
            ShippingTypeId = s.ShippingTypeId,
            Width = s.Width,
            Height = s.Height,
            Weight = s.Weight,
            Length = s.Length,
            PackageValue = s.PackageValue,
            PaymentMethodId = s.PaymentMethodId,
            // Sender
            SenderName = s.SenderName ?? string.Empty,
            SenderEmail = s.SenderEmail ?? string.Empty,
            SenderPhone = s.SenderPhone ?? string.Empty,
            SenderAddress = s.SenderAddress ?? string.Empty,
            SenderPostalCode = s.SenderPostalCode ?? string.Empty,
            SenderContact = s.SenderContact ?? string.Empty,
            SenderCityId = s.SenderCityId ?? Guid.Empty,
            // Receiver
            ReceiverName = s.ReceiverName ?? string.Empty,
            ReceiverEmail = s.ReceiverEmail ?? string.Empty,
            ReceiverPhone = s.ReceiverPhone ?? string.Empty,
            ReceiverAddress = s.ReceiverAddress ?? string.Empty,
            ReceiverPostalCode = s.ReceiverPostalCode ?? string.Empty,
            ReceiverContact = s.ReceiverContact ?? string.Empty,
            ReceiverCityId = s.ReceiverCityId ?? Guid.Empty,
        };

        await PopulateDropdownsAsync(vm);
        return View(vm);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ShipmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // ✅ التصحيح: نستخدم model.SenderId و model.ReceiverId بدلاً من CityId
        var requestDto = new UpdateShipmentRequestDto
        {
            SenderDto = new UpdateUserSenderDto
            {
                Name = model.SenderName,
                Email = model.SenderEmail,
                Phone = model.SenderPhone,
                PostalCode = model.SenderPostalCode,
                Contact = model.SenderContact,
                Address = model.SenderAddress,
                OtherAddress = model.SenderOtherAddress,
                CityId = model.SenderCityId,
                IsDefaultAddress = model.SenderIsDefault,
            },
            ReceiverDto = new UpdateUserReceiverDto
            {
                Name = model.ReceiverName,
                Email = model.ReceiverEmail,
                Phone = model.ReceiverPhone,
                PostalCode = model.ReceiverPostalCode,
                Contact = model.ReceiverContact,
                Address = model.ReceiverAddress,
                OtherAddress = model.ReceiverOtherAddress,
                CityId = model.ReceiverCityId,
                IsDefaultAddress = model.ReceiverIsDefault,
            },
            ShipmentDto = new UpdateShipmentDto
            {
                ShippingDate = model.ShippingDate,
                DeliveryDate = model.DeliveryDate,
                ShippingTypeId = model.ShippingTypeId,
                ShippingPackagingId = model.ShippingPackagingId,
                SenderId = model.SenderId,        // ✅ صح
                ReceiverId = model.ReceiverId,    // ✅ صح
                Width = model.Width,
                Height = model.Height,
                Weight = model.Weight,
                Length = model.Length,
                PackageValue = model.PackageValue,
                PaymentMethodId = model.PaymentMethodId,
                UserSubscriptionId = model.UserSubscriptionId,
            }
        };

        var result = await _shipmentService.UpdateShipment(id, requestDto);

        if (result.IsFailure)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        TempData["SuccessMessage"] = "Shipment updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // ================================================================
    // DELETE  — Admin only
    // ================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _shipmentService.DeleteAsync(id, autoSave: true);

        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
            result.IsSuccess
                ? "Shipment deleted successfully."
                : result.FirstError?.Description ?? "Failed to delete shipment.";

        return RedirectToAction(nameof(Index));
    }

    // ================================================================
    // APPROVE  — Admin, OpManager, Reviewer
    // ================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id, string? note = null)
    {
        var result = await _shipmentService.ApproveShipmentAsync(id, note);
        SetTransitionFeedback(result, "Shipment approved successfully.");
        return RedirectToAction(nameof(Details), new { id });
    }

    // ================================================================
    // MARK READY FOR SHIP  — Admin, OpManager, Op
    // ================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkReadyForShip(Guid id, string? note = null)
    {
        var result = await _shipmentService.MarkReadyForShipAsync(id, note);
        SetTransitionFeedback(result, "Shipment marked as ready for shipping.");
        return RedirectToAction(nameof(Details), new { id });
    }

    // ================================================================
    // MARK SHIPPED  — Admin, OpManager
    // ================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkShipped(Guid id, Guid carrierId, string? note = null)
    {
        if (carrierId == Guid.Empty)
        {
            TempData["ErrorMessage"] = "Please select a carrier before marking as shipped.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var dto = new ShipShipmentDto { CarrierId = carrierId, Note = note };
        var result = await _shipmentService.MarkShippedAsync(id, dto);
        SetTransitionFeedback(result, "Shipment marked as shipped.");
        return RedirectToAction(nameof(Details), new { id });
    }

    // ================================================================
    // PRIVATE HELPERS
    // ================================================================

    /// <summary>Converts a Result into a TempData success or error message.</summary>
    private void SetTransitionFeedback(Result result, string successMessage)
    {
        if (result.IsSuccess)
            TempData["SuccessMessage"] = successMessage;
        else
            TempData["ErrorMessage"] = result.FirstError?.Description ?? "Operation failed.";
    }

    /// <summary>Loads carriers as a SelectList for the MarkShipped dropdown.</summary>
    private async Task<List<SelectListItem>> LoadCarriersAsync()
    {
        var result = await _carrierService.GetAllAsync();
        if (result.IsFailure || result.Value is null)
            return new List<SelectListItem>();

        return result.Value.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.CarrierName
        }).ToList();
    }

    /// <summary>Populates all dropdown lists on the Create/Edit form.</summary>
    /// <summary>Populates all dropdown lists on the Create/Edit form.</summary>
    private async Task PopulateDropdownsAsync(ShipmentFormViewModel vm)
    {
        // جلب كل القوائم بشكل تسلسلي (بدون Task.WhenAll)
        var typesResult = await _shippingTypeService.GetAllAsync();
        var methodsResult = await _paymentMethodService.GetAllAsync();
        var citiesResult = await _cityService.GetAllAsync();
        var packagingResult = await _packagingService.GetAllAsync();

        // تعيين القوائم
        vm.ShippingTypes = (typesResult.IsSuccess ? typesResult.Value : Enumerable.Empty<BL.DTOs.ShippingType.ShippingTypeDto>())
            .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.ShippingTypeEname ?? t.ShippingTypeAname }).ToList();

        vm.PaymentMethods = (methodsResult.IsSuccess ? methodsResult.Value : Enumerable.Empty<BL.DTOs.PaymentMethod.PaymentMethodDto>())
            .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.MethodEname ?? m.MethdAname }).ToList();

        vm.Cities = (citiesResult.IsSuccess ? citiesResult.Value : Enumerable.Empty<BL.DTOs.City.CityDto>())
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CityEname ?? c.CityAname }).ToList();

        vm.PackagingTypes = (packagingResult.IsSuccess ? packagingResult.Value : Enumerable.Empty<BL.DTOs.ShippingPackaging.ShippingPackagingDto>())
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ShippingPackagingEname ?? p.ShippingPackagingAname }).ToList();
    }
}