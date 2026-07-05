using Domain.Entities;
using BL.DTOs.Shipment;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;
using BL.Contract.IServices.Shipment;
using BL.Common.Results;

namespace BL.Services.Shipment;

public class ShipmentService 
    : BaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>, IShipmentService
{

    private readonly IUserReceiverService _userReceiver;
    private readonly IUserSenderService _userSender;
    private readonly ITrackingNumberCalculator _trackingNumberCalculator;
    private readonly IRateCalculator _rateCalculator;

    public ShipmentService(IGenericRepository<TbShipment> repository, IMapper mapper, IUserService userService,
        IUserReceiverService userReceiver, IUserSenderService userSender, 
        ITrackingNumberCalculator trackingNumberCalculator, IRateCalculator rateCalculator)
        : base(repository, mapper, userService)
    {
        _userReceiver = userReceiver;
        _userSender = userSender;
        _trackingNumberCalculator = trackingNumberCalculator;
        _rateCalculator = rateCalculator;
    }

    public async Task<Result<Guid>> CreateShipment(CreateShipmentDto dto)
    {
        var validation = await ValidateSenderAndReceiverAsync(dto);
        
        if (validation.IsFailure)
            return validation.Errors.ToList();   

        dto.TrackingNumber = await _trackingNumberCalculator.GenerateTrackingNumber();
        dto.ShippingRate = await _rateCalculator.CalculateShippingRate(dto);

        return await CreateAsync(dto);
    }




    #region Private Methods
    private async Task<Result> ValidateSenderAndReceiverAsync(CreateShipmentDto dto)
    {
        if (dto.ReceiverId != Guid.Empty)
        {
            var receiver = await _userReceiver.GetByIdAsync(dto.ReceiverId);

            if (receiver is null)
                return Error.NotFound(
                    "Receiver.NotFound",
                    "Receiver was not found.");
        }

        if (dto.SenderId != Guid.Empty)
        {
            var sender = await _userSender.GetByIdAsync(dto.SenderId);

            if (sender is null)
                return Error.NotFound(
                    "Sender.NotFound",
                    "Sender was not found.");
        }

        return Result.Success();
    }
    #endregion
}