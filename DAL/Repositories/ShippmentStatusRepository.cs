using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class ShippmentStatusRepository : GenericRepository<TbShippmentStatus>, IShippmentStatusRepository
{
    public ShippmentStatusRepository(ShippingDbContext context, ILogger<ShippmentStatusRepository> logger) : base(context, logger)
    {
    }
}