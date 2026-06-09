using System;
using System.Collections.Generic;
using System.Text;

namespace BL.DTOs.Auth;

public class LoginDto
{
    public string UsernameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}