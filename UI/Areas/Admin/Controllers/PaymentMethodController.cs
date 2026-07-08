using BL.DTOs.PaymentMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Controllers;
using UI.Services;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PaymentMethodController : BaseMvcController<PaymentMethodDto, CreatePaymentMethodDto, UpdatePaymentMethodDto>
    {
        public PaymentMethodController(GenericApiClient apiClient, ILogger<PaymentMethodController> logger)
            : base(apiClient, logger, "Api/PaymentMethod")
        {
        }
    }
}
