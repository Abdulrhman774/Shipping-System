using DAL.Context;
using DAL.Contracts;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories;


public class RefreshTokenRepository : GenericRepository<TbRefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ShippingDbContext context, ILogger<RefreshTokenRepository> logger) : base(context, logger)
    {
    }

    public async Task<int> RevokeTokensAsync(string userId, CancellationToken cancellationToken = default)
    {
        var updatedBy = Guid.Parse(userId);

        return await _context.Set<TbRefreshToken>()
            .Where(rt => rt.UserId == userId &&
                         rt.CurrentState == enEntityState.Active)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.CurrentState, enEntityState.Inactive)
                .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                .SetProperty(x => x.UpdatedBy, updatedBy),
                cancellationToken);
    }
}