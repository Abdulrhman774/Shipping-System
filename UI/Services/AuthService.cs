using BL.Common;
using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UI.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    //public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
    //{
    //    var user = new ApplicationUser
    //    {
    //        FullName = dto.FullName,
    //        Email = dto.Email,
    //        UserName = dto.UserName,
    //        PhoneNumber = dto.PhoneNumber,
    //        Gender = dto.Gender,
    //        DateOfBirth = dto.DateOfBirth,
    //        ImageUrl = dto.ImageUrl
    //    };

    //    var result = await _userManager.CreateAsync(user, dto.Password);

    //    if (!result.Succeeded)
    //    {
    //        return new RegisterResponseDto
    //        {
    //            Success = false,
    //            //Errors = result.Errors.Select(e => e.Description).ToList()
    //        };
    //    }

    //    await _userManager.AddToRoleAsync(user, "User");

    //    return new RegisterResponseDto
    //    {
    //        Success = true,
    //        UserId = Guid.Parse(user.Id),
    //        Email = user.Email!,
    //        UserName = user.UserName!
    //    };
    //}

    //public async Task<Result<RegisterResponseDto>?> LoginAsync(LoginRequestDto dto)
    //{
    //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.UsernameOrEmail || x.Email == dto.UsernameOrEmail);

        
    //    if (user == null)
    //        return new RegisterResponseDto { Success = false };

    //    await _signInManager.SignInAsync(user, isPersistent: false);

    //    var result = await _signInManager
    //        .CheckPasswordSignInAsync(user, dto.Password, true);

    //    if (!result.Succeeded)
    //        return new RegisterResponseDto { Success = false };

    //    return new RegisterResponseDto
    //    {
    //        Success = true,
    //        UserId = Guid.Parse(user.Id),
    //        Email = user.Email!,
    //        UserName = user.UserName!
    //    };
    //}

    //public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequstDto dto)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null) return false;

    //    var result = await _userManager.ChangePasswordAsync(
    //        user,
    //        dto.CurrentPassword,
    //        dto.NewPassword);

    //    return result.Succeeded;
    //}

    //public async Task<bool> ResetPasswordAsync(string email)
    //{
    //    var user = await _userManager.FindByEmailAsync(email);
    //    if (user == null) return false;

    //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    //    var result = await _userManager.ResetPasswordAsync(user, token, "Temp@123");

    //    return result.Succeeded;
    //}

    //public async Task<RegisterResponseDto> LogoutAsync()
    //{
    //    await _signInManager.SignOutAsync();
    //    return new RegisterResponseDto { Success = true };
    //}

    public Task<IEnumerable<string>> GetRolesAsync(string userId)
    {
        throw new NotImplementedException();
    }

    Task<Result<TokenResponseDto>> IAuthService.LoginAsync(LoginRequestDto dto)
    {
        throw new NotImplementedException();
    }

    Task<Result<RegisterResponseDto>> IAuthService.RegisterAsync(RegisterRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> LogoutAsync(string userId)
    {
        throw new NotImplementedException();
    }

    Task<Result> IAuthService.ChangePasswordAsync(string Username, ChangePasswordRequstDto dto)
    {
        throw new NotImplementedException();
    }

    Task<Result> IAuthService.ResetPasswordAsync(string Username)
    {
        throw new NotImplementedException();
    }

    Task<Result<IEnumerable<string>>> IAuthService.GetRolesAsync(string userId)
    {
        throw new NotImplementedException();
    }
}