using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class UserReceiverRepository : GenericRepository<TbUserReceiver>, IUserReceiverRepository
{
    public UserReceiverRepository(ShippingDbContext context, ILogger<UserReceiverRepository> logger) : base(context, logger)
    {
    }
}