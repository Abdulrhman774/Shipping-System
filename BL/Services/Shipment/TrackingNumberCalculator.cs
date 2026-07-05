using BL.Contract.IServices.Shipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Shipment;

public class TrackingNumberCalculator : ITrackingNumberCalculator
{
    public Task<string> GenerateTrackingNumber()
    {
        throw new NotImplementedException();
    }
}
