using System;
using System.Collections.Generic;
using System.Text;

namespace BL.DTOs.Auth.Requests;

public class LoginRequestDto
{
    public string UsernameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}