using BL.Common.Results;
using BL.DTOs.Auth;
using BL.DTOs.User;


namespace BL.Contract.IServices;

public interface IUserService
{
    Task<Result> DeleteAccountAsync(string userId);

    Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();

    Task<Result<UserDto>> GetByIdAsync(string id);

    Task<Result> UpdateAsync(string updatedUserId, UpdateUserDto dto);

    Task<Guid> GetLoggedInUserAsync();

    Task<Result<UserDto>> GetUserByEmailOrUsernameAsync(string emailOrUsername);
}

