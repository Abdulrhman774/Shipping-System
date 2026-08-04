using DAL.Repositories.Generic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts.IRepositories;

public interface IShipmentStatusHistoryRepository : IGenericRepository<TbShipmentStatusHistory>
{
}
