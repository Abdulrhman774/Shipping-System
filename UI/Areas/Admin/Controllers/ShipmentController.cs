using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.Shippment;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ShipmentController : BaseController
    {
        // Constructor inject IShippmentService (commented out as per instructions)
        /*
        private readonly IShippmentService _shipmentService;
        public ShipmentController(IShippmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }
        */

        // GET: Admin/Shipment
        public IActionResult Index()
        {
            // Hardcoded list of shipments for display
            var shipments = new List<TbShippment>
            {
                new TbShippment
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    TrackingNumber = 10001001,
                    ShippingDate = new DateTime(2026, 6, 1, 10, 0, 0),
                    Sender = new TbUserSender { SenderName = "أحمد محمد" },
                    Receiver = new TbUserReceiver { ReceiverName = "سارة أحمد" },
                    ShippingType = new TbShippingType { ShippingTypeEname = "Express" },
                    ShippingRate = 150.00m,
                    CurrentState = enEntityState.Active
                },
                new TbShippment
                {
                    Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    TrackingNumber = 10001002,
                    ShippingDate = new DateTime(2026, 6, 3, 14, 30, 0),
                    Sender = new TbUserSender { SenderName = "خالد عبدالله" },
                    Receiver = new TbUserReceiver { ReceiverName = "علي حسن" },
                    ShippingType = new TbShippingType { ShippingTypeEname = "Standard" },
                    ShippingRate = 50.00m,
                    CurrentState = enEntityState.Active
                },
                new TbShippment
                {
                    Id = Guid.Parse("33333333-4444-5555-6666-777777777777"),
                    TrackingNumber = 10001003,
                    ShippingDate = new DateTime(2026, 6, 5, 9, 15, 0),
                    Sender = new TbUserSender { SenderName = "جون دو" },
                    Receiver = new TbUserReceiver { ReceiverName = "فاطمة عمر" },
                    ShippingType = new TbShippingType { ShippingTypeEname = "Same Day" },
                    ShippingRate = 250.00m,
                    CurrentState = enEntityState.Inactive
                }
            };

            return View(shipments);
        }

        // GET: Admin/Shipment/Details/{id}
        public IActionResult Details(Guid id)
        {
            // Hardcoded shipment details for display
            var shipment = new TbShippment
            {
                Id = id,
                TrackingNumber = 10001001,
                ShippingDate = new DateTime(2026, 6, 1, 10, 0, 0),
                Sender = new TbUserSender { SenderName = "أحمد محمد", Phone = "+201001234567" },
                Receiver = new TbUserReceiver { ReceiverName = "سارة أحمد", Phone = "+201009876543", Address = "شارع التحرير، الدقي، القاهرة" },
                ShippingType = new TbShippingType { ShippingTypeEname = "Express" },
                PaymentMethod = new TbPaymentMethod { MethodEname = "Credit Card" },
                Width = 10,
                Height = 15,
                Length = 20,
                Weight = 2.5,
                PackageValue = 1000m,
                ShippingRate = 150.00m,
                CurrentState = enEntityState.Active
            };

            // Hardcoded timeline status history
            var timeline = new List<dynamic>
            {
                new { Date = new DateTime(2026, 6, 1, 10, 5, 0), Status = "Created", Notes = "Shipment registered in system." },
                new { Date = new DateTime(2026, 6, 1, 14, 30, 0), Status = "Picked Up", Notes = "Picked up by courier from sender." },
                new { Date = new DateTime(2026, 6, 2, 8, 15, 0), Status = "In Transit", Notes = "Arrived at main sorting facility." }
            };

            ViewBag.Timeline = timeline;
            return View(shipment);
        }

        // GET: Admin/Shipment/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View(new CreateShippmentDto { ShippingDate = DateTime.Now });
        }

        // POST: Admin/Shipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateShippmentDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Shipment/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            var types = new List<TbShippingType>
            {
                new TbShippingType { Id = Guid.NewGuid(), ShippingTypeEname = "Standard" },
                new TbShippingType { Id = Guid.NewGuid(), ShippingTypeEname = "Express" },
                new TbShippingType { Id = Guid.NewGuid(), ShippingTypeEname = "Same Day" }
            };
            ViewBag.ShippingTypes = new SelectList(types, "Id", "ShippingTypeEname");

            var payments = new List<TbPaymentMethod>
            {
                new TbPaymentMethod { Id = Guid.NewGuid(), MethodEname = "Cash on Delivery" },
                new TbPaymentMethod { Id = Guid.NewGuid(), MethodEname = "Credit Card" }
            };
            ViewBag.PaymentMethods = new SelectList(payments, "Id", "MethodEname");

            var senders = new List<TbUserSender>
            {
                new TbUserSender { Id = Guid.NewGuid(), SenderName = "أحمد محمد" },
                new TbUserSender { Id = Guid.NewGuid(), SenderName = "خالد عبدالله" }
            };
            ViewBag.Senders = new SelectList(senders, "Id", "SenderName");

            var receivers = new List<TbUserReceiver>
            {
                new TbUserReceiver { Id = Guid.NewGuid(), ReceiverName = "سارة أحمد" },
                new TbUserReceiver { Id = Guid.NewGuid(), ReceiverName = "علي حسن" }
            };
            ViewBag.Receivers = new SelectList(receivers, "Id", "ReceiverName");
        }
    }
}
