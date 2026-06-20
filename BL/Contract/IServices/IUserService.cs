using BL.DTOs.Auth;
using BL.DTOs.User;


namespace BL.Contract.IServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<Guid> GetLoggedInUserAsync();
    Task<UserDto> GetByIdAsync(string userId);
    Task<UserDto> GetUserByEmailOrUsernameAsync(string emailOrUsername);
    Task<bool> UpdateAsync(string updatedUserId, UpdateUserDto dto);
    Task<bool> DeleteAccountAsync(string userId);
}

