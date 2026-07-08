using BL.DTOs.ShipmentStatus;
using BL.DTOs.Shipment;
using BL.DTOs.Carrier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Controllers;
using UI.Services;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ShipmentStatusController : BaseMvcController<ShipmentStatusDto, CreateShipmentStatusDto, UpdateShipmentStatusDto>
    {
        public ShipmentStatusController(GenericApiClient apiClient, ILogger<ShipmentStatusController> logger)
            : base(apiClient, logger, "Api/ShipmentStatus")
        {
        }

        public new async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View(new CreateShipmentStatusDto());
        }

        public override async Task<IActionResult> Edit(Guid id)
        {
            await LoadDropdownsAsync();
            var response = await _apiClient.GetAsync<ShipmentStatusDto>($"{_apiEndpoint}/{id}");
            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Error ?? "Item not found.";
                return RedirectToAction(nameof(Index));
            }

            var dto = response.Data;
            var updateDto = new UpdateShipmentStatusDto
            {
                ShipmentId = dto.ShipmentId,
                Notes = dto.Notes,
                CarrierId = dto.CarrierId
            };

            ViewBag.Id = id;
            return View(updateDto);
        }

        private async Task LoadDropdownsAsync()
        {
            var shipmentsResponse = await _apiClient.GetAsync<IEnumerable<ShipmentDto>>("Api/Shipment");
            var shipments = shipmentsResponse.Success && shipmentsResponse.Data != null ? shipmentsResponse.Data : new List<ShipmentDto>();
            ViewBag.Shipments = new SelectList(shipments, "Id", "TrackingNumber");

            var carriersResponse = await _apiClient.GetAsync<IEnumerable<CarrierDto>>("Api/Carrier");
            var carriers = carriersResponse.Success && carriersResponse.Data != null ? carriersResponse.Data : new List<CarrierDto>();
            ViewBag.Carriers = new SelectList(carriers, "Id", "CarrierName");
        }
    }
}
