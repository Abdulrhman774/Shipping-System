using BL.Common;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;
using BL.DTOs.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contract.IServices;

public interface IAuthService
{
    Task<ApiResponse<RegisterResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<RegisterResponseDto> LogoutAsync();
    Task<bool> ChangePasswordAsync(string Username, ChangePasswordRequstDto dto);
    Task<bool> ResetPasswordAsync(string Username);
    Task<IEnumerable<string>> GetRolesAsync(string userId);
}
