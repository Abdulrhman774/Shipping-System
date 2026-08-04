using BL.Contract.IServices;
using BL.DTOs.ShipmentStatusHistory;
using BL.DTOs.ShippingPackaging;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class ShipmentStatusHistory : BaseService<TbShipmentStatusHistory, ShipmentStatusHistoryDto, CreateShipmentStatusHistoryDto, UpdateShipmentStatusHistoryDto>, IShipmentStatusHistory
{
    public ShipmentStatusHistory(IUnitOfWork unitOfWork, IBaseMapper mapper, IUserService userService) : base(unitOfWork, mapper, userService)
    {
    }

}
