using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class ShippmentStatusRepository : GenericRepository<TbShippmentStatus>, IShippmentStatusRepository
{
    public ShippmentStatusRepository(AppDbContext context, ILogger<ShippmentStatusRepository> logger) : base(context, logger)
    {
    }
}