using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Domain.Entities;
using BL.DTOs.PaymentMethod;
using BL.Contract.IServices;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PaymentMethodController : Controller
    {
        // Constructor inject IPaymentMethodService (commented out as per instructions)
        /*
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        */

        // GET: Admin/PaymentMethod
        public IActionResult Index()
        {
            // Hardcoded list of 3 sample payment methods for display
            var paymentMethods = new List<TbPaymentMethod>
            {
                new TbPaymentMethod
                {
                    Id = Guid.Parse("11111111-d1d1-d1d1-d1d1-111111111111"),
                    MethdAname = "الدفع عند الاستلام",
                    MethodEname = "Cash on Delivery",
                    Commission = 5.0,
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 1, 15, 10, 0, 0)
                },
                new TbPaymentMethod
                {
                    Id = Guid.Parse("22222222-e2e2-e2e2-e2e2-222222222222"),
                    MethdAname = "بطاقة الائتمان",
                    MethodEname = "Credit Card",
                    Commission = 2.5,
                    CurrentState = enEntityState.Active,
                    CreatedDate = new DateTime(2026, 2, 8, 14, 30, 0)
                },
                new TbPaymentMethod
                {
                    Id = Guid.Parse("33333333-f3f3-f3f3-f3f3-333333333333"),
                    MethdAname = "محفظة إلكترونية",
                    MethodEname = "E-Wallet",
                    Commission = 0.0,
                    CurrentState = enEntityState.Inactive,
                    CreatedDate = new DateTime(2026, 3, 22, 9, 15, 0)
                }
            };

            return View(paymentMethods);
        }

        // GET: Admin/PaymentMethod/Create
        public IActionResult Create()
        {
            return View(new CreatePaymentMethodDto());
        }

        // POST: Admin/PaymentMethod/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePaymentMethodDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/PaymentMethod/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            // Hardcoded update model with pre-filled values
            var dto = new UpdatePaymentMethodDto
            {
                MethdAname = "الدفع عند الاستلام",
                MethodEname = "Cash on Delivery",
                Commission = 5.0
            };
            return View(dto);
        }

        // POST: Admin/PaymentMethod/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, UpdatePaymentMethodDto dto)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/PaymentMethod/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            // Redirect to Index (no actual business logic)
            return RedirectToAction(nameof(Index));
        }
    }
}
