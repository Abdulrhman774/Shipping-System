using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class CityRepository : GenericRepository<TbCity>, ICityRepository
{
    public CityRepository(AppDbContext context, ILogger<CityRepository> logger) : base(context, logger)
    {
    }
}
