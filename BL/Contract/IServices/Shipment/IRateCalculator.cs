using BL.DTOs.Shipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract.IServices.Shipment;

public interface IRateCalculator
{
    Task<decimal> CalculateShippingRate(CreateShipmentDto createShipmentDto);
}
