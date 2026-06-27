using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.User;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor   )
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> DeleteAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        return Result.Success();
    }
    public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        var users = await _userManager.Users
            .Select(u => _MapToDto(u))
            .ToListAsync();

        return users;
    }
    public async Task<Result<UserDto>> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");

        return _MapToDto(user);
    }
    public async Task<Result> UpdateAsync(string updatedUserId, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(updatedUserId);

        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");

        user.FullName = dto.FullName;
        user.PhoneNumber = dto.PhoneNumber;
        user.ImageUrl = dto.ImageUrl ?? user.ImageUrl;
        user.DateOfBirth = dto.DateOfBirth;
        user.Gender = dto.Gender;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        return Result.Success();
    }
    public async Task<Result<UserDto>> GetUserByEmailOrUsernameAsync(string emailOrUsername)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Email == emailOrUsername ||
                u.UserName == emailOrUsername);
    
        if (user is null)
            return Error.NotFound(
                "User.NotFound",
                "User was not found.");
    
        return _MapToDto(user);
    }
    public async Task<Guid> GetLoggedInUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty;
    }
    private static UserDto _MapToDto(ApplicationUser u) => new()
    {
        Id = Guid.Parse(u.Id),
        FullName = u.FullName,
        Email = u.Email!,
        PhoneNumber = u.PhoneNumber,
        ImageUrl = u.ImageUrl,
        Gender = u.Gender,
        DateOfBirth = u.DateOfBirth,
    };
}
