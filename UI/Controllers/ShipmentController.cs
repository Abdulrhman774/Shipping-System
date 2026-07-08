using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using BL.Services;
using BL.Services.Shipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using UI.Models.Shipment;

namespace UI.Controllers
{
    [Authorize]
    public class ShipmentController : Controller
    {
        private readonly ICityService _cityService;
        private readonly IShippingTypeService _shippingTypeService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IShippingPackagingService _shippingPackagingService;
        private readonly IUserSenderService _userSenderService;
        private readonly IUserReceiverService _userReceiverService;
        private readonly IShipmentService _shipmentService;

        public ShipmentController(
            ICityService cityService,
            IShippingTypeService shippingTypeService,
            IPaymentMethodService paymentMethodService,
            IShippingPackagingService shippingPackagingService,
            IUserSenderService userSenderService,
            IUserReceiverService userReceiverService,
            IShipmentService shipmentService)
        {
            _cityService = cityService;
            _shippingTypeService = shippingTypeService;
            _paymentMethodService = paymentMethodService;
            _shippingPackagingService = shippingPackagingService;
            _userSenderService = userSenderService;
            _userReceiverService = userReceiverService;
            _shipmentService = shipmentService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            return Guid.Empty;
        }

        private async Task<bool> PopulateDropdownDataAsync(CreateShipmentViewModel model)
        {
            var citiesResult = await _cityService.GetAllAsync();
            var shippingTypesResult = await _shippingTypeService.GetAllAsync();
            var packagingResult = await _shippingPackagingService.GetAllAsync();
            var paymentMethodsResult = await _paymentMethodService.GetAllAsync();

            if (citiesResult.IsFailure || shippingTypesResult.IsFailure ||
                packagingResult.IsFailure || paymentMethodsResult.IsFailure)
            {
                return false;
            }

            model.Cities = citiesResult.Value
                .Select(c => new SelectListItem(c.CityAname ?? c.Id.ToString(), c.Id.ToString())).ToList();

            model.ShippingTypes = shippingTypesResult.Value
                .Select(s => new SelectListItem(s.ShippingTypeEname, s.Id.ToString())).ToList();

            model.PackagingTypes = packagingResult.Value
                .Select(p => new SelectListItem(p.ShippingPackagingEname, p.Id.ToString())).ToList();

            model.PaymentMethods = paymentMethodsResult.Value
                .Select(p => new SelectListItem(p.MethodEname, p.Id.ToString())).ToList();

            return true;
        }
        [HttpGet]
        public async Task<IActionResult> CreateShipment()
        {
            var model = new CreateShipmentViewModel();
            var success = await PopulateDropdownDataAsync(model);
            
            if (!success)
            {
                return View("Error");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShipment(CreateShipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var success = await PopulateDropdownDataAsync(model);
                if (!success) return View("Error");
                return View(model);
            }

            var currentUserId = GetCurrentUserId();

            // 1. Resolve Sender
            Guid senderId;
            if (model.Sender.ExistingId.HasValue)
            {
                senderId = model.Sender.ExistingId.Value;
            }
            else
            {
                var senderDto = new CreateUserSenderDto
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
                };
                
                var senderResult = await _userSenderService.CreateAsync(senderDto);
                if (senderResult.IsFailure)
                {
                    return View("Error", senderResult.Errors);
                }
                senderId = senderResult.Value;
            }

            // 2. Resolve Receiver
            Guid receiverId;
            if (model.Receiver.ExistingId.HasValue)
            {
                receiverId = model.Receiver.ExistingId.Value;
            }
            else
            {
                var receiverDto = new CreateUserReceiverDto
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
                };

                var receiverResult = await _userReceiverService.CreateAsync(receiverDto);
                if (receiverResult.IsFailure)
                {
                    return View("Error", receiverResult.Errors);
                }
                receiverId = receiverResult.Value;
            }

            // 3. Create Shipment Dto
            var shipmentDto = new CreateShipmentDto
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

            var result = await _shipmentService.CreateShipment(shipmentDto);

            if (result.IsFailure)
            {
                return View("Error", result.Errors);
            }

            return RedirectToAction("Confirmation", new { id = result.Value });
        }
        
        [HttpGet]
        public IActionResult Confirmation(Guid id)
        {
            return View(id);
        }
    }
}
