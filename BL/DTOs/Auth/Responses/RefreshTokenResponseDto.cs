using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Auth.Responses;

/// <summary>
/// Returned by POST /Api/Auth/RefreshToken.
/// Contains both a new AccessToken and a rotated RefreshToken so the
/// caller can update whatever storage mechanism it uses.
/// </summary>
public sealed class RefreshTokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}