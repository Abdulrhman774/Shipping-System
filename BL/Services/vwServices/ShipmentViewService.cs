using BL.Common.Results;
using BL.Contract.IvwServices;
using BL.DTOs.Views;
using BL.Mapping;
using DAL.Contracts.IRepositories;
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
}