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

    public async Task<bool> DeleteAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.Select(u => _MapToDto(u));

        return await users.ToListAsync();
    }

    public async Task<UserDto> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        return _MapToDto(user);
    }


    public async Task<bool> UpdateAsync(string updatedUserId, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(updatedUserId.ToString());
        if (user == null) return false;

        // Update user properties
        user.FullName = dto.FullName;
        user.PhoneNumber = dto.PhoneNumber;
        user.ImageUrl = dto.ImageUrl ?? user.ImageUrl;
        user.DateOfBirth = dto.DateOfBirth;
        user.Gender = dto.Gender;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
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

    public async Task<Guid> GetLoggedInUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty;
    }

    public async Task<UserDto> GetUserByEmailOrUsernameAsync(string emailOrUsername)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Email == emailOrUsername || u.UserName == emailOrUsername);

        if (user == null) return null;

        return _MapToDto(user);


    }
}
