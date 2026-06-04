using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class CarrierRepository : GenericRepository<TbCarrier>, ICarrierRepository
{
    public CarrierRepository(AppDbContext context, ILogger<CarrierRepository> logger) : base(context, logger)
    {
    }
}