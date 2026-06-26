using BL.Common.Results;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;

namespace BL.Contract.IServices;

public interface IAuthService
{
    Task<Result<TokenResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<RegisterResponseDto> LogoutAsync();
    Task<bool> ChangePasswordAsync(string Username, ChangePasswordRequstDto dto);
    Task<bool> ResetPasswordAsync(string Username);
    Task<IEnumerable<string>> GetRolesAsync(string userId);
}
