using DAL.Context;
using DAL.Contracts.IRepositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class SettingRepository :  ISettingRepository
{
    public SettingRepository(ShippingDbContext context, ILogger<SettingRepository> logger)
    {
    }
}