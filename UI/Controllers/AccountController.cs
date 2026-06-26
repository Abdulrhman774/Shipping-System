using BL.Contract.IServices;
using BL.DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UI.Models;
using UI.Models.ResponsesModels;
using UI.Services;

namespace UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly GenericApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;
        public readonly Services.AuthenticationService _authenticationService;

        public AccountController(
            GenericApiClient apiClient,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger,
            Services.AuthenticationService authenticationService)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // 1. تحويل ViewModel → DTO
                var loginDto = new LoginRequestDto
                {
                    UsernameOrEmail = model.UsernameOrEmail,
                    Password = model.Password
                };

                // 2. استدعاء Service (كل المنطق هناك)
                var (success, principal, error, loginData) = await _authenticationService.LoginAsync(loginDto);

                if (!success || principal == null || loginData == null)
                {
                    ModelState.AddModelError("", error ?? "Invalid username or password.");
                    return View(model);
                }

                // 3. تسجيل الدخول (مسؤولية Controller)
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                };

                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    authProperties);

                // 4. تخزين AccessToken في Session
                HttpContext.Session.SetString("AccessToken", loginData.AccessToken);

                // 5. تخزين RefreshToken في Cookie آمن
                Response.Cookies.Append("RefreshToken", loginData.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                _logger.LogInformation($"User {model.UsernameOrEmail} logged in successfully");

                // 6. إعادة التوجيه
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "AdminPanel", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Username}", model.UsernameOrEmail);
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View(model);
            }
        }
        // GET: /Account/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // 1. Prepare registration data
                var registerDto = new RegisterRequestDto
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

                // 2. Call API to register - returns ApiResponse<RegisterResponseData>
                var response = await _apiClient.PostAsync<RegisterResponseData>("Api/Auth/Register", registerDto);

                // 3. Check if registration failed
                if (!response.Success)
                {
                    // If response has error message
                    if (!string.IsNullOrEmpty(response.Error))
                    {
                        ModelState.AddModelError("", response.Error);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Registration failed. Please try again.");
                    }
                    return View(model);
                }

                _logger.LogInformation($"User {model.Email} registered successfully");
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for user {Email}", model.Email);
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View(model);
            }
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Get token from session
                var token = HttpContext.Session.GetString("AccessToken");

                if (!string.IsNullOrEmpty(token))
                {
                    // Set token in header and call API logout
                    _apiClient.SetAuthorizationHeader(token);
                    await _apiClient.PostAsync<object>("Api/Auth/logout", null);
                    _apiClient.ClearAuthorizationHeader();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "API logout failed, but continuing with local logout");
            }

            // Clear local session and cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/AccessDenied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: /Account/Profile
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Get user ID from claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction(nameof(Login));

                // Call API to get user profile - returns ApiResponse<UserProfileResponseData>
                var response = await _apiClient.GetAsync<UserProfileResponseData>($"Api/Users/{userId}");

                // Check if profile fetch failed
                if (!response.Success || response.Data == null)
                {
                    TempData["ErrorMessage"] = response.Error ?? "Failed to load profile.";
                    return View("Error");
                }

                return View(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load profile");
                return View("Error");
            }
        }

        // GET: /Account/RefreshToken
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                // Get current token from session
                var token = HttpContext.Session.GetString("AccessToken");
                _apiClient.SetAuthorizationHeader(token);

                // Call API to refresh token - returns ApiResponse<RefreshTokenResponseData>
                var response = await _apiClient.PostAsync<RefreshTokenResponseData>("Api/Auth/Refresh-Token", null);

                // Check if refresh succeeded
                if (response.Success && response.Data != null && !string.IsNullOrEmpty(response.Data.AccessToken))
                {
                    // Update session with new token
                    HttpContext.Session.SetString("AccessToken", response.Data.AccessToken);

                    // Update user claims with new token
                    await UpdateUserClaims(response.Data.AccessToken);

                    return Ok(new { success = true, message = "Token refreshed successfully" });
                }

                return Unauthorized(new { success = false, message = response.Error ?? "Failed to refresh token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh token");
                return Unauthorized(new { success = false, message = "Failed to refresh token" });
            }
        }

        #region Helper Methods

        /// <summary>
        /// Update user claims with new token
        /// </summary>
        private async Task UpdateUserClaims(string newToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(newToken);

            var claims = new List<Claim>();

            // Add all claims from token
            foreach (var claim in jwtToken.Claims)
            {
                claims.Add(claim);
            }

            // Add AccessToken claim
            claims.Add(new Claim("AccessToken", newToken));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign out and sign in with new claims
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        #endregion
    }
}