using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories;

public class ShipmentStatusHistoryRepository : GenericRepository<TbShipmentStatusHistory>, IShipmentStatusHistoryRepository
{
    public ShipmentStatusHistoryRepository(ShippingDbContext context, ILogger<GenericRepository<TbShipmentStatusHistory>> logger) : base(context, logger)
    {
    }
}

