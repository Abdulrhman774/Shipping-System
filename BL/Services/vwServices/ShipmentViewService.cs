using BL.Common;
using BL.Common.Results;
using BL.Contract.IvwServices;
using BL.DTOs.Views;
using BL.Mapping;
using DAL.Contracts.IRepositories;
using Domain.Entities.Views.Dashboard;
using Domain.Entities.Views.Shipment.Statistics_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.vwServices;

public class ShipmentViewService : IShipmentViewService
{
    private readonly IShipmentViewRepository _repository;
    private readonly IBaseMapper _mapper;

    public ShipmentViewService(IShipmentViewRepository repository, IBaseMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ShipmentDetailsDto>>> GetShipmentDetailsAsync()
    {
        var data = await _repository.GetShipmentDetailsAsync();
        return _mapper.MapList<vw_ShipmentDetails, ShipmentDetailsDto>(data);
    }

    public async Task<Result<ShipmentDetailsDto>> GetShipmentDetailByIdAsync(Guid id)
    {
        var data = await _repository.GetShipmentDetailByIdAsync(id);
        if (data == null)
            return Error.NotFound("Shipment.NotFound", "Shipment not found.");
        return _mapper.Map<vw_ShipmentDetails, ShipmentDetailsDto>(data);
    }

    public async Task<Result<IEnumerable<ShipmentDetailsDto>>> GetShipmentsByUserAsync(Guid userId)
    {
        var data = await _repository.GetShipmentsByUserAsync(userId);
        return _mapper.MapList<vw_ShipmentDetails, ShipmentDetailsDto>(data);
    }

    public async Task<Result<ShipmentDetailsDto>> GetShipmentByTrackingNumberAsync(string trackingNumber, Guid CreatedByUserId)
    {
        var data = await _repository.GetShipmentByTrackingNumberAsync(trackingNumber, CreatedByUserId);
        if (data == null)
            return Error.NotFound("Shipment.NotFound", "Shipment not found.");
        return _mapper.Map<vw_ShipmentDetails, ShipmentDetailsDto>(data);
    }

    public async Task<Result<IEnumerable<ShipmentStatsDto>>> GetShipmentStatsAsync()
    {
        var data = await _repository.GetShipmentStatsAsync();
        return _mapper.MapList<vw_ShipmentStats, ShipmentStatsDto>(data);
    }

    public async Task<Result<IEnumerable<MonthlyShipmentsDto>>> GetMonthlyShipmentsAsync()
    {
        var data = await _repository.GetMonthlyShipmentsAsync();
        return _mapper.MapList<VwMonthlyShipments, MonthlyShipmentsDto>(data);
    }

    public async Task<Result<IEnumerable<ShipmentsByTypeDto>>> GetShipmentsByTypeAsync()
    {
        var data = await _repository.GetShipmentsByTypeAsync();
        return _mapper.MapList<VwShipmentsByType, ShipmentsByTypeDto>(data);
    }

    public async Task<Result<PagedResult<ShipmentDetailsDto>>> GetShipmentDetailsPagedAsync(int pageNumber, int pageSize)
    {
        var (data, totalCount) = await _repository.GetPagedShipmentDetailsAsync(pageNumber, pageSize);
        var dtoList = _mapper.MapList<vw_ShipmentDetails, ShipmentDetailsDto>(data);
        var result = new PagedResult<ShipmentDetailsDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        return Result<PagedResult<ShipmentDetailsDto>>.Success(result);
    }

    public async Task<Result<PagedResult<ShipmentDetailsDto>>> GetShipmentsByUserPagedAsync(int pageNumber, int pageSize, Guid userId)
    {
        var (data, totalCount) = await _repository.GetShipmentsByUserPagedAsync(pageNumber, pageSize, userId);
        var dtoList = _mapper.MapList<vw_ShipmentDetails, ShipmentDetailsDto>(data);
        var result = new PagedResult<ShipmentDetailsDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        return Result<PagedResult<ShipmentDetailsDto>>.Success(result);
    }

    public async Task<Result<AdminDashboardDto>> GetDashboardStatsAsync()
    {
        var (total, active, pending, delayed, recent) = await _repository.GetDashboardStatsAsync();

        var recentDtos = _mapper.MapList<vw_ShipmentDetails, ShipmentDetailsDto>(recent);

        // Calculate efficiency rate based on your business logic. For now, we will use a placeholder value.
        double efficiency = 94.2;

        var dto = new AdminDashboardDto
        {
            TotalShipments = total,
            ActiveDeliveries = active,
            PendingApprovals = pending,
            EfficiencyRate = efficiency,
            DelayedShipments = delayed,
            RecentShipments = recentDtos
        };

        return Result<AdminDashboardDto>.Success(dto);
    }

    public async Task<Result<VwDashboardSummary>> GetDashboardSummaryAsync()
    {
        try
        {
            var data = await _repository.GetDashboardSummaryAsync();
            if (data is null)
                return Error.NotFound("Dashboard.NotFound", "No dashboard summary available.");
            return Result<VwDashboardSummary>.Success(data);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("Dashboard.Error", $"Failed to load dashboard summary: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<VwRecentShipment>>> GetRecentShipmentsAsync(int count = 20)
    {
        try
        {
            var data = await _repository.GetRecentShipmentsAsync(count);
            return Result<IEnumerable<VwRecentShipment>>.Success(data);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("RecentShipments.Error", $"Failed to load recent shipments: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<VwShipmentStatusDistribution>>> GetStatusDistributionAsync()
    {
        try
        {
            var data = await _repository.GetStatusDistributionAsync();
            return Result<IEnumerable<VwShipmentStatusDistribution>>.Success(data);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("StatusDistribution.Error", $"Failed to load status distribution: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<VwMonthlyVolume>>> GetMonthlyVolumeAsync()
    {
        try
        {
            var data = await _repository.GetMonthlyVolumeAsync();
            return Result<IEnumerable<VwMonthlyVolume>>.Success(data);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("MonthlyVolume.Error", $"Failed to load monthly volume: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<VwTopShipper>>> GetTopShippersAsync(int top = 5)
    {
        try
        {
            var data = await _repository.GetTopShippersAsync(top);
            return Result<IEnumerable<VwTopShipper>>.Success(data);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("TopShippers.Error", $"Failed to load top shippers: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<VwFinancial>>> GetMonthlyFinancialsAsync()
    {
        try
        {
            var data = await _repository.GetMonthlyFinancialsAsync();
            return Result<IEnumerable<VwFinancial>>.Success(data);
        }
        catch (Exception ex)
        {
            return Error.Unexpected("Financials.Error", $"Failed to load financial data: {ex.Message}");
        }
    }
}