using Domain.Entities.Views.Shipment.Statistics_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts.IRepositories
{
    public interface IShipmentViewRepository
    {
        Task<IEnumerable<vw_ShipmentDetails>> GetShipmentDetailsAsync();
        Task<vw_ShipmentDetails?> GetShipmentDetailByIdAsync(Guid id);
        Task<IEnumerable<vw_ShipmentDetails>> GetShipmentsByUserAsync(Guid userId);
        Task<vw_ShipmentDetails?> GetShipmentByTrackingNumberAsync(string trackingNumber, Guid CreatedByUserId);
        Task<IEnumerable<vw_ShipmentStats>> GetShipmentStatsAsync();
        Task<IEnumerable<VwMonthlyShipments>> GetMonthlyShipmentsAsync();
        Task<IEnumerable<VwShipmentsByType>> GetShipmentsByTypeAsync();
    }
}
