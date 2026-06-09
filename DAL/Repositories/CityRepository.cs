using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class CityRepository : GenericRepository<TbCity>, ICityRepository
{
    public CityRepository(ShippingDbContext context, ILogger<CityRepository> logger) : base(context, logger)
    {
    }
}
