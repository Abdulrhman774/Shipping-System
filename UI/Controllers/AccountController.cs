using BL.Contract.IServices;
using BL.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Map ViewModel → DTO
            var loginDto = new LoginDto
            {
                Email = model.UsernameOrEmail,
                Password = model.Password
            };

            var result = await _authService.LoginAsync(loginDto);

            if (result == null || !result.Success)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            // Handle ReturnUrl
            if (!string.IsNullOrEmpty(model.ReturnUrl)
                && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Map ViewModel → DTO
            var registerDto = new RegisterDto
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                ImageUrl = ""
            };

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                foreach (var error in result.Errors ?? new List<string>())
                    ModelState.AddModelError("", error);
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
