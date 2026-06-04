using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class UserSenderRepository : GenericRepository<TbUserSender>, IUserSenderRepository
{
    public UserSenderRepository(AppDbContext context, ILogger<UserSenderRepository> logger) : base(context, logger)
    {
    }
}
