using BL.Common.Results;
using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services.Shipment;

public class ShipmentService
    : BaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>, IShipmentService
{
    private readonly IUserReceiverService _userReceiver;
    private readonly IUserSenderService _userSender;
    private readonly ITrackingNumberCalculator _trackingNumberCalculator;
    private readonly IRateCalculator _rateCalculator;
    private readonly IDistanceService _distanceService;

    private readonly IShippingTypeService _shippingTypeService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IUserSubscriptionService _userSubscriptionService;
    private readonly IShippingPackagingService _shippingPackagingService;
    private readonly IUserService _userService;

    public ShipmentService(
        IGenericRepository<TbShipment> repository,
        IMapper mapper,
        IUserService userService,
        IUserReceiverService userReceiver,
        IUserSenderService userSender,
        ITrackingNumberCalculator trackingNumberCalculator,
        IRateCalculator rateCalculator,
        IDistanceService distanceService,
        IUnitOfWork unitOfWork,
        IShippingTypeService shippingTypeService,
        IPaymentMethodService paymentMethodService,
        IUserSubscriptionService userSubscriptionService,
        IShippingPackagingService shippingPackagingService)
        : base(unitOfWork, mapper, userService)
    {
        _userReceiver = userReceiver;
        _userSender = userSender;
        _trackingNumberCalculator = trackingNumberCalculator;
        _rateCalculator = rateCalculator;
        _distanceService = distanceService;
        _shippingTypeService = shippingTypeService;
        _paymentMethodService = paymentMethodService;
        _userSubscriptionService = userSubscriptionService;
        _shippingPackagingService = shippingPackagingService;
        _userService = userService;
    }

    public async Task<Result<Guid>> CreateShipment(CreateShipmentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Get logged-in user
            var userId = await _userService.GetLoggedInUserAsync();
            if (string.IsNullOrEmpty(userId.ToString()))
                return Error.Unauthorized("User.NotLoggedIn", "User must be logged in to create a shipment.");

            // 2. Validate all references
            var validationResult = await ValidateShipmentReferencesAsync(dto);
            if (validationResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return validationResult.Errors.ToList();
            }

            // 3. Validate sender and receiver
            var senderReceiverValidation = await ValidateSenderAndReceiverAsync(dto);
            if (senderReceiverValidation.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return senderReceiverValidation.Errors.ToList();
            }

            // 4. Handle subscription if applicable
            bool isUsingSubscription = false;
            if (dto.UserSubscriptionId.HasValue && dto.UserSubscriptionId.Value != Guid.Empty)
            {
                var subscriptionCheck = await _rateCalculator.TryConsumeFromSubscriptionAsync(
                    dto.UserSubscriptionId.Value, dto);

                if (subscriptionCheck.IsFailure)
                {
                    // If subscription check fails, fall back to standard rate
                    dto.UserSubscriptionId = null;
                }
                else if (subscriptionCheck.Value)
                {
                    isUsingSubscription = true;
                }
            }

            // 5. Generate tracking number
            dto.TrackingNumber = await _trackingNumberCalculator.GenerateTrackingNumber();

            // 6. Calculate shipping rate
            var rateResult = await _rateCalculator.CalculateShippingRateAsync(dto);
            if (rateResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return rateResult.Errors.ToList();
            }
            dto.ShippingRate = rateResult.Value;

            // 7. Create the shipment
            var result = await CreateAsync(dto);
            if (result.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return result;
            }

            // 8. If using subscription, update usage
            if (isUsingSubscription && dto.UserSubscriptionId.HasValue)
            {
                var distance = await _distanceService.GetDistanceBetweenSenderAndReceiverAsync(
                    dto.SenderId, dto.ReceiverId);

                var updateResult = await _rateCalculator.UpdateSubscriptionUsageAsync(
                    dto.UserSubscriptionId.Value, dto, distance);

                if (updateResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return updateResult.Errors.ToList();
                }
            }

            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    #region Private Methods

    private async Task<Result> ValidateShipmentReferencesAsync(CreateShipmentDto dto)
    {
        var errors = new List<Error>();

        // Validate ShippingTypeId
        if (dto.ShippingTypeId == Guid.Empty)
            errors.Add(Error.Validation("ShippingType.Required", "Shipping type is required."));
        else
        {
            var shippingType = await _shippingTypeService.GetByIdAsync(dto.ShippingTypeId);
            if (shippingType.IsFailure || shippingType.Value is null)
                errors.Add(Error.NotFound("ShippingType.NotFound", "Shipping type was not found."));
        }

        // Validate PaymentMethodId (optional)
        if (dto.PaymentMethodId.HasValue && dto.PaymentMethodId.Value != Guid.Empty)
        {
            var paymentMethod = await _paymentMethodService.GetByIdAsync(dto.PaymentMethodId.Value);
            if (paymentMethod.IsFailure || paymentMethod.Value is null)
                errors.Add(Error.NotFound("PaymentMethod.NotFound", "Payment method was not found."));
        }

        // Validate UserSubscriptionId (optional)
        if (dto.UserSubscriptionId.HasValue && dto.UserSubscriptionId.Value != Guid.Empty)
        {
            var subscription = await _userSubscriptionService.GetByIdAsync(dto.UserSubscriptionId.Value);
            if (subscription.IsFailure || subscription.Value is null)
                errors.Add(Error.NotFound("UserSubscription.NotFound", "User subscription was not found."));
        }

        // Validate ShippingPackagingId (optional)
        if (dto.ShippingPackagingId.HasValue && dto.ShippingPackagingId.Value != Guid.Empty)
        {
            var packaging = await _shippingPackagingService.GetByIdAsync(dto.ShippingPackagingId.Value);
            if (packaging.IsFailure || packaging.Value is null)
                errors.Add(Error.NotFound("ShippingPackaging.NotFound", "Shipping packaging was not found."));
        }

        // Validate ReferenceId (optional)
        if (dto.ReferenceId.HasValue && dto.ReferenceId.Value != Guid.Empty)
        {
            var referenceShipment = await GetByIdAsync(dto.ReferenceId.Value);
            if (referenceShipment.IsFailure || referenceShipment.Value is null)
                errors.Add(Error.NotFound("ReferenceShipment.NotFound", "Reference shipment was not found."));
        }

        if (errors.Any())
            return Result.Failure(errors);

        return Result.Success();
    }

    private async Task<Result> ValidateSenderAndReceiverAsync(CreateShipmentDto dto)
    {
        // Validate SenderId
        if (dto.SenderId == Guid.Empty)
            return Error.Validation("Sender.Required", "Sender is required.");

        var sender = await _userSender.GetByIdAsync(dto.SenderId);
        if (sender is null || sender.IsFailure)
            return Error.NotFound("Sender.NotFound", "Sender was not found.");

        // Validate ReceiverId
        if (dto.ReceiverId == Guid.Empty)
            return Error.Validation("Receiver.Required", "Receiver is required.");

        var receiver = await _userReceiver.GetByIdAsync(dto.ReceiverId);
        if (receiver is null || receiver.IsFailure)
            return Error.NotFound("Receiver.NotFound", "Receiver was not found.");

        // Check if sender and receiver are the same
        if (dto.SenderId == dto.ReceiverId)
            return Error.Validation("SameSenderReceiver", "Sender and receiver cannot be the same person.");

        return Result.Success();
    }

    public Task<Result<ShipmentDto>> GetShipmentByTrackingNumberAsync(string trackingNumber)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<ShipmentDto>>> GetShipmentsForUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    #endregion
}