using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Auth.Requests;

/// <summary>
/// Sent by any caller (MVC, React, Flutter, Postman, …) when they need
/// to exchange a RefreshToken for new tokens.
/// The API has zero knowledge of where the caller stored the token.
/// </summary>
public sealed class RefreshTokenRequestDto
{
    /// <summary>The opaque RefreshToken string previously issued by the API.</summary>
    public string RefreshToken { get; set; } = string.Empty;
}