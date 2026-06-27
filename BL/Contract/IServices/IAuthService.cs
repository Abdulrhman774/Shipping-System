using BL.Common.Results;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;

namespace BL.Contract.IServices;

public interface IAuthService
{
    Task<Result<TokenResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto);
    Task<Result> LogoutAsync(string userId);
    Task<Result> ChangePasswordAsync(string Username, ChangePasswordRequstDto dto);
    Task<Result> ResetPasswordAsync(string Username);
    Task<Result<IEnumerable<string>>> GetRolesAsync(string userId);
}
