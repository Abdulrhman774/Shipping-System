using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class SettingRepository : GenericRepository<TbSetting>, ISettingRepository
{
    public SettingRepository(ShippingDbContext context, ILogger<SettingRepository> logger) : base(context, logger)
    {
    }
}