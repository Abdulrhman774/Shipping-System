using BL.Contract;
using BL.DTOs.ShipmentStatusHistory;
using BL.DTOs.UserSender;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract.IServices
{
    internal interface IShipmentStatusHistory : IBaseService<TbShipmentStatusHistory, ShipmentStatusHistoryDto, CreateShipmentStatusHistoryDto, UpdateShipmentStatusHistoryDto>
    {
    }
}
