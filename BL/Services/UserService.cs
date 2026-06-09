//using BL.Contract.IServices;
//using BL.DTOs.User;
//using Domain.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace BL.Services;

//public class UserService : IUserService
//{
//    private readonly UserManager<ApplicationUser> _userManager;
//    public UserService(UserManager<ApplicationUser> userManager)
//    {
//        _userManager = userManager;
//    }

//    public async Task<bool> DeleteAccountAsync(string userId)
//    {
//        var user = await _userManager.FindByIdAsync(userId);
//        if (user == null) return false;

//        var result = await _userManager.DeleteAsync(user);

//        return result.Succeeded;
//    }

//    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
//    {
//        var users = _userManager.Users.Select(u => MapToDto(u));

//        return await users.ToListAsync();
//    }

//    public async Task<UserDto> GetByIdAsync(string id)
//    {
//        var user = await _userManager.FindByIdAsync(id);
//        if (user == null) return null;

//        return MapToDto(user);
//    }


//    public async Task<bool> UpdateAsync(string updatedUserId, UpdateUserDto dto)
//    {
//        var user = await _userManager.FindByIdAsync(updatedUserId);
//        if (user == null) return false;

//        // Update user properties
//        user.FullName = dto.FullName;
//        user.PhoneNumber = dto.PhoneNumber;
//        user.ImageUrl = dto.ImageUrl ?? user.ImageUrl;
//        user.DateOfBirth = dto.DateOfBirth;
//        user.Gender = dto.Gender;

//        var result = await _userManager.UpdateAsync(user);

//        return result.Succeeded;
//    }

//    private static UserDto MapToDto(ApplicationUser u) => new()
//    {
//        Id = u.Id,
//        FullName = u.FullName,
//        Email = u.Email!,
//        PhoneNumber = u.PhoneNumber,
//        ImageUrl = u.ImageUrl,
//        Gender = u.Gender,
//        DateOfBirth = u.DateOfBirth,
//    };
//}
