using BL.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.RefreshToken;

public class RefreshTokenDto : BaseDto
{
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime Expires { get; set; }
}
