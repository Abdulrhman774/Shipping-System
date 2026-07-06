using BL.Common.Results;
using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using BL.Mapping;
using DAL.Contracts;
using DAL.Repositories.Generic;
using Domain.Entities;

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
        ITrackingNumberCalculator trackingNumberCalculator, IRateCalculator rateCalculator, IUnitOfWork unitOfWork)
        : base(unitOfWork, mapper, userService)
    {
        _userReceiver = userReceiver;
        _userSender = userSender;
        _trackingNumberCalculator = trackingNumberCalculator;
        _rateCalculator = rateCalculator;
    }

    public async Task<Result<Guid>> CreateShipment(CreateShipmentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var validation = await ValidateSenderAndReceiverAsync(dto);
            if (validation.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return validation.Errors.ToList();
            }

            dto.TrackingNumber = await _trackingNumberCalculator.GenerateTrackingNumber();
            dto.ShippingRate = await _rateCalculator.CalculateShippingRate(dto);

            var result = await CreateAsync(dto);

            if (result.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return result;
            }

            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw; // أو ترجع Error.Failure عام حسب استراتيجية التعامل مع الأخطاء عندك
        }
    }




    #region Private Methods
    private async Task<Result> ValidateSenderAndReceiverAsync(CreateShipmentDto dto)
    {
        if (dto.ReceiverId == Guid.Empty)
            return Error.Validation("Receiver.Required", "Receiver is required.");
    
        var receiver = await _userReceiver.GetByIdAsync(dto.ReceiverId);
        if (receiver is null)
            return Error.NotFound("Receiver.NotFound", "Receiver was not found.");
    
        if (dto.SenderId == Guid.Empty)
            return Error.Validation("Sender.Required", "Sender is required.");
    
        var sender = await _userSender.GetByIdAsync(dto.SenderId);
        if (sender is null)
            return Error.NotFound("Sender.NotFound", "Sender was not found.");
    
        return Result.Success();
    }
    
    #endregion
}