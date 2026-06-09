using BL.DTOs.Auth;
using BL.DTOs.User;


namespace BL.Contract.IServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<Guid> GetLoggedInUserAsync();
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<bool> UpdateAsync(Guid updatedUserId, UpdateUserDto dto);
    Task<bool> DeleteAccountAsync(Guid userId);
}

