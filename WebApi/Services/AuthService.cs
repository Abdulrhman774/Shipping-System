using BL.Common;
using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;
using BL.DTOs.RefreshToken;
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


    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto)
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
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description)).ToList();
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
        {
            return roleResult.Errors
                .Select(e => Error.Failure(
                    e.Code,
                    e.Description))
                .ToList();
        }

        return new RegisterResponseDto
        {
            UserId = Guid.Parse(user.Id),
            Email = user.Email!,
            UserName = user.UserName!
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

        var claims = await _tokenService.BuildClaimsAsync(user);
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

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequstDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");

        var result = await _userManager.ChangePasswordAsync(
            user,
            dto.CurrentPassword,
            dto.NewPassword);

        if (!result.Succeeded)
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(
            user,
            token,
            "Temp@123");

        if (!result.Succeeded)
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();

        return Result.Success();
    }

    public async Task<Result> LogoutAsync(string userId)
    {
        var revoked = await _refreshService.RevokeTokensAsync(userId);

        if (!revoked)
            return Error.Unexpected(
                "RefreshToken.RevokeFailed",
                "Failed to revoke refresh tokens.");

        await _signInManager.SignOutAsync();

        return Result.Success();
    }



    public async Task<Result<IEnumerable<string>>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Error.NotFound("User.NotFound", "User was not found.");

        var roles = await _userManager.GetRolesAsync(user);

        return Result<IEnumerable<string>>.Success(roles);
    }


}