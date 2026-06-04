using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class UserReceiverRepository : GenericRepository<TbUserReceiver>, IUserReceiverRepository
{
    public UserReceiverRepository(AppDbContext context, ILogger<UserReceiverRepository> logger) : base(context, logger)
    {
    }
}