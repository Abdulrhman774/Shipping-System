using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Auth;

public class _RegisterResponseData
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public string UserId { get; set; } = null!;
    //public List<string>? Errors { get; set; }

}
