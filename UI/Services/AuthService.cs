using BL.Contract.IServices;
using BL.DTOs.Auth;
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

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
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
            return new AuthResponseDto
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _userManager.AddToRoleAsync(user, "User");

        return new AuthResponseDto
        {
            Success = true,
            UserId = Guid.Parse(user.Id),
            FullName = user.FullName,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.UsernameOrEmail || x.Email == dto.UsernameOrEmail);

        
        if (user == null)
            return new AuthResponseDto { Success = false };

        await _signInManager.SignInAsync(user, isPersistent: false);

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, dto.Password, true);

        if (!result.Succeeded)
            return new AuthResponseDto { Success = false };

        return new AuthResponseDto
        {
            Success = true,
            UserId = Guid.Parse(user.Id),
            FullName = user.FullName,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
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

    public async Task<AuthResponseDto> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return new AuthResponseDto { Success = true };
    }

    public Task<IEnumerable<string>> GetRolesAsync(string userId)
    {
        throw new NotImplementedException();
    }
}