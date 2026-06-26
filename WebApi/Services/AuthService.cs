using BL.Common;
using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;
using BL.DTOs.User;
using BL.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly TokenService _tokenService;
    private readonly IRefreshTokenService _refreshService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, TokenService tokenService, IRefreshTokenService refreshService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _refreshService = refreshService;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        var user = new ApplicationUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.UserName,
            PhoneNumber = dto.PhoneNumber,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            ImageUrl = dto.ImageUrl
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return new RegisterResponseDto
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(user, "User");

        return new RegisterResponseDto
        {
            Success = true,
            UserId = Guid.Parse(user.Id),
            //FullName = user.FullName,
            Email = user.Email!,
            UserName = user.UserName!
            //PhoneNumber = user.PhoneNumber!,
            //Gender = user.Gender,
            //ProfileImage = user.ImageUrl
        };

    }

    public async Task<Result<TokenResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.UsernameOrEmail || x.Email == dto.UsernameOrEmail);
            

        if (user == null)
            return Error.Unauthorized("Auth.InvalidCredentials", "Invalid username or email.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return Error.Forbidden("Auth.LockedOut", "Account is locked.");
            if (result.IsNotAllowed)
                return Error.Unauthorized("Auth.NotAllowed", "Account is not confirmed.");


            return Error.Unauthorized("Auth.InvalidPassword", "Invalid password.");
        }

        var claims = await BuildClaimsAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();


        bool SaveSuccessfully = await _refreshService.SaveTokenAsync(user.Id.ToString(), refreshToken, DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpiryDays()));

        if (!SaveSuccessfully) return Error.Unexpected("RefreshToken.SaveFailed", "Failed to save refresh token.");


        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Email = user.Email!,
            UserId = Guid.Parse(user.Id),
            ExpiresAt = DateTime.UtcNow.AddMinutes(
            _tokenService.GetAccessTokenExpiryMinutes())
        };
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequstDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(
            user,
            dto.CurrentPassword,
            dto.NewPassword);

        return result.Succeeded;
    }

    public async Task<bool> ResetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, "Temp@123");

        return result.Succeeded;
    }

    public async Task<RegisterResponseDto> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return new RegisterResponseDto();
    }

    public async Task<IEnumerable<string>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();
        return await _userManager.GetRolesAsync(user);
    }

    #region Private Method
    private async Task<Claim[]> BuildClaimsAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email,           user.Email ?? "Email not found"),
            new Claim(ClaimTypes.Role,           role)
        };
    }

    #endregion


    #region Custome Methods

    public async Task<ApiResponse<RefreshTokenResponseDto>> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return ApiResponse<RefreshTokenResponseDto>.BadRequest("Refresh token is required.");

        var stored = await _refreshService.GetTokenAsync(refreshToken);

        if (stored is null)
            return ApiResponse<RefreshTokenResponseDto>.Unauthorized("Refresh token not found.");

        if (stored.CurrentState is not enEntityState.Active)
            return ApiResponse<RefreshTokenResponseDto>.Unauthorized("Refresh token has been revoked.");

        if (stored.Expires < DateTime.UtcNow)
            return ApiResponse<RefreshTokenResponseDto>.Unauthorized("Refresh token has expired.");

        // Revoke before issuing new pair (rotation prevents replay attacks).
        await _refreshService.RevokeTokensAsync(stored.UserId);

        var user = await _userManager.FindByIdAsync(stored.UserId);
        if (user is null)
            return ApiResponse<RefreshTokenResponseDto>.Unauthorized("User no longer exists.");

        var claims = await BuildClaimsAsync(user);
        var newAccessToken = _tokenService.GenerateAccessToken(claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var saved = await _refreshService.SaveTokenAsync(
            stored.UserId,
            newRefreshToken,
            DateTime.UtcNow.AddDays(_tokenService.GetRefreshTokenExpiryDays()));

        if (!saved)
            return ApiResponse<RefreshTokenResponseDto>.BadRequest("Failed to persist new refresh token.");

        return ApiResponse<RefreshTokenResponseDto>.Ok(new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenService.GetAccessTokenExpiryMinutes()),
        });
    }

    public async Task<ApiResponse<RefreshAccessTokenResponseDto>> RefreshAccessTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return ApiResponse<RefreshAccessTokenResponseDto>.BadRequest("Refresh token is required.");

        var stored = await _refreshService.GetTokenAsync(refreshToken);

        if (stored is null)
            return ApiResponse<RefreshAccessTokenResponseDto>.Unauthorized("Refresh token not found.");

        if (stored.CurrentState is not enEntityState.Active)
            return ApiResponse<RefreshAccessTokenResponseDto>.Unauthorized("Refresh token has been revoked.");

        if (stored.Expires < DateTime.UtcNow)
            return ApiResponse<RefreshAccessTokenResponseDto>.Unauthorized("Refresh token has expired.");

        // Do NOT revoke — only a new AccessToken is needed.
        var user = await _userManager.FindByIdAsync(stored.UserId);
        if (user is null)
            return ApiResponse<RefreshAccessTokenResponseDto>.Unauthorized("User no longer exists.");

        var claims = await BuildClaimsAsync(user);
        var newAccessToken = _tokenService.GenerateAccessToken(claims);

        return ApiResponse<RefreshAccessTokenResponseDto>.Ok(new RefreshAccessTokenResponseDto
        {
            AccessToken = newAccessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenService.GetAccessTokenExpiryMinutes()),
        });
    }

    #endregion


}