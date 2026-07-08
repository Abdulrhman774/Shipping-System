using BL.DTOs.UserSubscription;
using BL.DTOs.SubscriptionPackage;
using BL.DTOs.UserSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Controllers;
using UI.Services;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserSubscriptionController : BaseMvcController<UserSubscriptionDto, CreateUserSubscriptionDto, UpdateUserSubscriptionDto>
    {
        public UserSubscriptionController(GenericApiClient apiClient, ILogger<UserSubscriptionController> logger)
            : base(apiClient, logger, "Api/UserSubscription")
        {
        }

        public new async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View(new CreateUserSubscriptionDto { SubscriptionDate = DateTime.UtcNow });
        }

        public override async Task<IActionResult> Edit(Guid id)
        {
            await LoadDropdownsAsync();
            var response = await _apiClient.GetAsync<UserSubscriptionDto>($"{_apiEndpoint}/{id}");
            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Error ?? "Item not found.";
                return RedirectToAction(nameof(Index));
            }

            var dto = response.Data;
            var updateDto = new UpdateUserSubscriptionDto
            {
                UserId = dto.UserId,
                PackageId = dto.PackageId,
                SubscriptionDate = dto.SubscriptionDate
            };

            ViewBag.Id = id;
            return View(updateDto);
        }

        private async Task LoadDropdownsAsync()
        {
            var packagesResponse = await _apiClient.GetAsync<IEnumerable<SubscriptionPackageDto>>("Api/SubscriptionPackage");
            var packages = packagesResponse.Success && packagesResponse.Data != null ? packagesResponse.Data : new List<SubscriptionPackageDto>();
            ViewBag.Packages = new SelectList(packages, "Id", "PackageName");

            var usersResponse = await _apiClient.GetAsync<IEnumerable<UserSenderDto>>("Api/UserSender");
            var users = usersResponse.Success && usersResponse.Data != null ? usersResponse.Data : new List<UserSenderDto>();
            ViewBag.Users = new SelectList(users, "Id", "SenderName");
        }
    }
}
