using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Services.Simulation;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
