using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract.IServices;

public interface IDistanceService
{
    Task<decimal> GetDistanceBetweenCitiesAsync(Guid fromCityId, Guid toCityId);
    Task<decimal> GetDistanceBetweenSenderAndReceiverAsync(Guid senderId, Guid receiverId);
}
