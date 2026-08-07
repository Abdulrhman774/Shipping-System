using BL.Common;
using BL.Common.Results;
using BL.DTOs.Views;
using Domain.Entities.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract.IvwServices;

public interface IShipmentViewService
{
    Task<Result<IEnumerable<ShipmentDetailsDto>>> GetShipmentDetailsAsync();
    Task<Result<ShipmentDetailsDto>> GetShipmentDetailByIdAsync(Guid id);
    Task<Result<IEnumerable<ShipmentDetailsDto>>> GetShipmentsByUserAsync(Guid userId);
    Task<Result<ShipmentDetailsDto>> GetShipmentByTrackingNumberAsync(string trackingNumber, Guid CreatedByUserId);
    Task<Result<IEnumerable<ShipmentStatsDto>>> GetShipmentStatsAsync();
    Task<Result<IEnumerable<MonthlyShipmentsDto>>> GetMonthlyShipmentsAsync();
    Task<Result<IEnumerable<ShipmentsByTypeDto>>> GetShipmentsByTypeAsync();
    Task<Result<PagedResult<ShipmentDetailsDto>>> GetShipmentDetailsPagedAsync(int pageNumber, int pageSize);
    Task<Result<PagedResult<ShipmentDetailsDto>>> GetShipmentsByUserPagedAsync(int pageNumber, int pageSize, Guid userId);

    Task<Result<AdminDashboardDto>> GetDashboardStatsAsync();


    Task<Result<VwDashboardSummary>> GetDashboardSummaryAsync();
    Task<Result<IEnumerable<VwRecentShipment>>> GetRecentShipmentsAsync(int count = 20);
    Task<Result<IEnumerable<VwShipmentStatusDistribution>>> GetStatusDistributionAsync();
    Task<Result<IEnumerable<VwMonthlyVolume>>> GetMonthlyVolumeAsync();
    Task<Result<IEnumerable<VwTopShipper>>> GetTopShippersAsync(int top = 5);
    Task<Result<IEnumerable<VwFinancial>>> GetMonthlyFinancialsAsync();
}
