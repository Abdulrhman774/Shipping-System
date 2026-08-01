using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts;

public interface IRefreshTokenRepository : IGenericRepository<TbRefreshToken>
{
    Task<int> RevokeTokensAsync(string userId, CancellationToken cancellationToken = default);
}
