using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class UserSubscriptionRepository : GenericRepository<TbUserSubscription>, IUserSubscriptionRepository
{
    public UserSubscriptionRepository(ShippingDbContext context, ILogger<UserSubscriptionRepository> logger) : base(context, logger)
    {
    }
}