using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class ShippmentRepository : GenericRepository<TbShippment>, IShippmentRepository
{
    public ShippmentRepository(AppDbContext context, ILogger<ShippmentRepository> logger) : base(context, logger)
    {
    }
}