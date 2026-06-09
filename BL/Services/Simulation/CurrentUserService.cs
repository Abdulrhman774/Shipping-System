using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Services.Simulation
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserId { get; }

        public CurrentUserService()
        {
            UserId = Guid.NewGuid();
        }
    }
}
