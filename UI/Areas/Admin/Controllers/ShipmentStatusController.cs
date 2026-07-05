using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Entities;
using BL.DTOs.ShipmentStatus;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ShipmentStatusController : BaseController
    {
        // Constructor inject IShippmentStatusService (commented out as per instructions)
        /*
        private readonly IShippmentStatusService _shipmentStatusService;
        public ShipmentStatusController(IShippmentStatusService shipmentStatusService)
        {
            _shipmentStatusService = shipmentStatusService;
        }
        */

        // GET: Admin/ShipmentStatus
        public IActionResult Index()
        {
            // Hardcoded list of shipment statuses for display
            var statuses = new List<TbShipmentStatus>
            {
                new TbShipmentStatus
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    ShipmentId = Guid.Parse("aaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                    Shippment = new TbShipment { TrackingNumber = "10001001" },
                    CarrierId = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    Carrier = new TbCarrier { CarrierName = "DHL Express" },
                    Notes = "Shipment picked up and processed at the origin hub.",
                    CreatedDate = new DateTime(2026, 6, 5, 10, 30, 0),
                    CurrentState = enEntityState.Active
                },
                new TbShipmentStatus
                {
                    Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    ShipmentId = Guid.Parse("fffffff-eeee-dddd-cccc-bbbbbbbbbbbb"),
                    Shippment = new TbShipment { TrackingNumber = "10001002" },
                    CarrierId = Guid.Parse("33333333-4444-5555-6666-777777777777"),
                    Carrier = new TbCarrier { CarrierName = "FedEx" },
                    Notes = "In transit to the destination country facility.",
                    CreatedDate = new DateTime(2026, 6, 6, 8, 15, 0),
                    CurrentState = enEntityState.Active
                }
            };

            return View(statuses);
        }

        // GET: Admin/ShipmentStatus/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View(new CreateShipmentStatusDto());
        }

        // POST: Admin/ShipmentStatus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateShipmentStatusDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ShipmentStatus/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            var shipments = new List<TbShipment>
            {
                new TbShipment { Id = Guid.Parse("aaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), TrackingNumber = "10001001" },
                new TbShipment { Id = Guid.Parse("fffffff-eeee-dddd-cccc-bbbbbbbbbbbb"), TrackingNumber = "10001002" }
            };
            // Displaying tracking number as text
            ViewBag.Shipments = new SelectList(shipments, "Id", "TrackingNumber");

            var carriers = new List<TbCarrier>
            {
                new TbCarrier { Id = Guid.Parse("22222222-3333-4444-5555-666666666666"), CarrierName = "DHL Express" },
                new TbCarrier { Id = Guid.Parse("33333333-4444-5555-6666-777777777777"), CarrierName = "FedEx" }
            };
            ViewBag.Carriers = new SelectList(carriers, "Id", "CarrierName");
        }
    }
}
