using System;
using System.Collections.Generic;
using System.Text;

namespace BL.DTOs.Auth.Requests
{
    public class ChangePasswordRequstDto
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
