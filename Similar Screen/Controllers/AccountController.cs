using BL.Contract;
using BL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ui.Models;
using Ui.Services;

namespace Ui.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;
        private readonly GenericApiClient _apiClient;
        public AccountController(IUserService userService, GenericApiClient apiClient)
        {
            _userService = userService;
            _apiClient = apiClient;
        }
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return View("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await _userService.LoginAsync(user);
            if (result.Success)
            {
                // Call the login API using the generic client
                LoginApiModel apiResult = await _apiClient.PostAsync<LoginApiModel>("api/auth/login", user);

                if (apiResult == null)
                {
                    ModelState.AddModelError(string.Empty, "API error: Unable to process login.");
                    return View(user);
                }

                var accessToken = apiResult?.AccessToken.ToString();

                if (string.IsNullOrEmpty(accessToken))
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(user);
                }
                // Store the access token in the cookie (for subsequent requests)
                Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(15)  // Adjust token expiry based on your needs
                });

                Response.Cookies.Append("RefreshToken", apiResult?.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7)  // Adjust token expiry based on your needs
                });

                var dbUser = await _userService.GetUserByEmailAsync(user.Email);

                if(dbUser.Role.ToLower()=="admin")
                    return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
                else
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
                return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserDto user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await _userService.RegisterAsync(user);
            if (result.Success)
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }
            else
                return View();
        }
    }
}
