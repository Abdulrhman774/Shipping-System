using BL.DTOs.SubscriptionPackage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Controllers;
using UI.Services;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SubscriptionPackageController : BaseMvcController<SubscriptionPackageDto, CreateSubscriptionPackageDto, UpdateSubscriptionPackageDto>
    {
        public SubscriptionPackageController(GenericApiClient apiClient, ILogger<SubscriptionPackageController> logger)
            : base(apiClient, logger, "Api/SubscriptionPackage")
        {
        }
    }
}
