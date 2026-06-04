using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class UserSubscriptionRepository : GenericRepository<TbUserSubscription>, IUserSubscriptionRepository
{
    public UserSubscriptionRepository(AppDbContext context, ILogger<UserSubscriptionRepository> logger) : base(context, logger)
    {
    }
}