using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class SubscriptionPackageRepository : GenericRepository<TbSubscriptionPackage>, ISubscriptionPackageRepository
{
    public SubscriptionPackageRepository(AppDbContext context, ILogger<SubscriptionPackageRepository> logger) : base(context, logger)
    {
    }
}