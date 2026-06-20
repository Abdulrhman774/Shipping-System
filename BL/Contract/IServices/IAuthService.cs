using BL.Common;
using BL.DTOs.Auth;
using BL.DTOs.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contract.IServices;

public interface IAuthService
{
    Task<ApiResponse<TokenResponseDto>> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LogoutAsync();
    Task<bool> ChangePasswordAsync(string Username, ChangePasswordDto dto);
    Task<bool> ResetPasswordAsync(string Username);
    Task<IEnumerable<string>> GetRolesAsync(string userId);
}
