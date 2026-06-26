using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Auth.Responses;

/// <summary>
/// Returned by POST /Api/Auth/Refresh-AccessToken.
/// Only a new AccessToken is issued; the RefreshToken is NOT rotated.
/// Used by the MVC app after an app-restart wipes the Session.
/// </summary>
public sealed class RefreshAccessTokenResponseDto
{
    public string   AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt   { get; set; }
}