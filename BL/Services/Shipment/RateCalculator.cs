using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Shipment;

public class RateCalculator : IRateCalculator
{
    public Task<decimal> CalculateShippingRate(CreateShipmentDto createShipmentDto)
    {
        throw new NotImplementedException();
    }
}
