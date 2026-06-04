using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class SettingRepository :  ISettingRepository
{
    public SettingRepository(AppDbContext context, ILogger<SettingRepository> logger)
    {
    }
}