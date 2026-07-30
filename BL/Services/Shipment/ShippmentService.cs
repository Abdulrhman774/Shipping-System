using BL.Common.Results;
using BL.Contract.IServices;
using BL.Contract.IServices.Shipment;
using BL.DTOs.Shipment;
using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using BL.Mapping;
using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using System.Reflection;

namespace BL.Services.Shipment;

public class ShipmentService
    : BaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>, IShipmentService
{

    private readonly ITrackingNumberCalculator _trackingNumberCalculator;
    private readonly IRateCalculator _rateCalculator;
    private readonly IDistanceService _distanceService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IUserSubscriptionService _userSubscriptionService;
    private readonly IShippingPackagingService _shippingPackagingService;
    private readonly IUserSenderService _userSenderService;
    private readonly IUserReceiverService _userReceiverService;
    private readonly IGenericRepository<TbShippingType> _shippingTypeRepository;
    private readonly ShippingDbContext _shippingContext;

    public ShipmentService(
        ShippingDbContext shippingContext,
        IMapper mapper,
        IUserService userService,
        ITrackingNumberCalculator trackingNumberCalculator,
        IRateCalculator rateCalculator,
        IDistanceService distanceService,
        IUnitOfWork unitOfWork,
        IUserSenderService userSenderservice,
        IUserReceiverService userReceiverService,
        IPaymentMethodService paymentMethodService,
        IUserSubscriptionService userSubscriptionService,
        IShippingPackagingService shippingPackagingService)
        : base(unitOfWork, mapper, userService)
    {
        _trackingNumberCalculator = trackingNumberCalculator;
        _rateCalculator = rateCalculator;
        _distanceService = distanceService;
        _paymentMethodService = paymentMethodService;
        _userSubscriptionService = userSubscriptionService;
        _shippingPackagingService = shippingPackagingService;
        _userSenderService = userSenderservice;
        _userReceiverService = userReceiverService;
        _shippingContext = shippingContext;

        // ✅ Get repository from UnitOfWork
        _shippingTypeRepository = unitOfWork.Repository<TbShippingType>();
    }




    public async Task<Result<Guid>> CreateShipment(CreateShipmentDto dto, CreateUserSenderDto senderDto, CreateUserReceiverDto receiverDto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = await _userService.GetLoggedInUserAsync();
            if (string.IsNullOrEmpty(userId.ToString()))
                return Error.Unauthorized("User.NotLoggedIn", "User must be logged in to create a shipment.");

            // 1. Create Sender ✅
            var senderResult = await _userSenderService.AddAsync(senderDto);
            if (senderResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return senderResult.Errors.ToList();
            }
            var sender = senderResult.Value; // ✅ الـ Entity المتتبع

            // 2. Create Receiver ✅
            var receiverResult = await _userReceiverService.AddAsync(receiverDto);
            if (receiverResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return receiverResult.Errors.ToList();
            }
            var receiver = receiverResult.Value; // ✅ الـ Entity المتتبع

            
            // 4-8. باقي المنطق (References, Distance, Subscription, Rate, Tracking)
            // ...

            // 9. Create Shipment ✅
            var shipment = _mapper.Map<CreateShipmentDto, TbShipment>(dto);
            shipment.Sender = sender;    
            shipment.Receiver = receiver;

            // 10. Add Shipment ✅
            await _repository.CreateAsync(shipment); // NO SAVE

            // 11. Update Subscription
            // ...

            // 12. SAVE EVERYTHING ✅
            await _unitOfWork.CommitTransactionAsync();

            return Result<Guid>.Success(shipment.Id);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<Result<Guid>> CreateShipment(CreateShipmentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = await _userService.GetLoggedInUserAsync();
            if (string.IsNullOrEmpty(userId.ToString()))
                return Error.Unauthorized("User.NotLoggedIn", "User must be logged in to create a shipment.");

            // 1. Create Sender ✅
            var senderResult = await _userSenderService.AddAsync(dto.Sender);
            if (senderResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return senderResult.Errors.ToList();
            }
            var sender = senderResult.Value; // ✅ الـ Entity المتتبع

            // 2. Create Receiver ✅
            var receiverResult = await _userReceiverService.AddAsync(dto.Receiver);
            if (receiverResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return receiverResult.Errors.ToList();
            }
            var receiver = receiverResult.Value; // ✅ الـ Entity المتتبع

            
            // 4-8. باقي المنطق (References, Distance, Subscription, Rate, Tracking)
            // ...

            // 9. Create Shipment ✅
            var shipment = _mapper.Map<CreateShipmentDto, TbShipment>(dto);
            shipment.Sender = sender;    
            shipment.Receiver = receiver;

            // 10. Add Shipment ✅
            await _repository.CreateAsync(shipment); // NO SAVE

            // 11. Update Subscription
            // ...

            // 12. SAVE EVERYTHING ✅
            await _unitOfWork.CommitTransactionAsync();

            return Result<Guid>.Success(shipment.Id);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<Result<Guid>> CreateShipmentv2(CreateShipmentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = await _userService.GetLoggedInUserAsync();
            if (string.IsNullOrEmpty(userId.ToString()))
                return Error.Unauthorized("User.NotLoggedIn", "User must be logged in to create a shipment.");

            // ================================================================
            // 1. Create Sender Entity (NO SAVE - just tracked by DbContext)
            // ================================================================

            var senderResult = await _userSenderService.AddAsync(dto.Sender);

            if (senderResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return senderResult.Errors.ToList();
            }

            var sender = senderResult.Value; // ✅ Tracked entity


            // ✅ DON'T assign dto.SenderId yet - we'll use navigation property

            // ================================================================
            // 2. Create Receiver Entity (NO SAVE - just tracked by DbContext)
            // ================================================================
            var receiverResult = await _userReceiverService.AddAsync(dto.Receiver);
                        
            if (receiverResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return receiverResult.Errors.ToList();
            }

            var receiver = receiverResult.Value; // ✅ Tracked entity

            // ✅ DON'T assign dto.ReceiverId yet - we'll use navigation property

            // ================================================================
            // 3. Validate References (ShippingType, PaymentMethod, etc.)
            // ================================================================
            var referencesResult = await ValidateShipmentReferencesAsync(dto);
            if (referencesResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return referencesResult.Errors.ToList();
            }
            var shippingType = referencesResult.Value;

            // ================================================================
            // 4. Get Distance (using navigation properties - no IDs needed)
            // ================================================================
            var distance = await _distanceService.GetDistanceBetweenCitiesAsync(
                dto.Sender.CityId,   // ✅ Use the entity directly
                dto.Receiver.CityId  // ✅ Use the entity directly
            );

            // ================================================================
            // 5. Subscription (Optional)
            // ================================================================
            TbUserSubscription? consumedSubscription = null;

            if (dto.UserSubscriptionId is { } subId && subId != Guid.Empty)
            {
                var subscriptionResult = await _rateCalculator.TryConsumeFromSubscriptionAsync(subId, dto, distance);

                if (subscriptionResult.IsSuccess)
                    consumedSubscription = subscriptionResult.Value;
                else
                    dto.UserSubscriptionId = null;
            }

            // ================================================================
            // 6. Calculate Rate
            // ================================================================
            decimal rate;
            if (consumedSubscription is not null)
            {
                rate = 0m;
            }
            else
            {
                var rateResult = await _rateCalculator.CalculateStandardRateAsync(dto, shippingType, distance);
                if (rateResult.IsFailure)
                {
                    await _shippingContext.Database.RollbackTransactionAsync();
                    return rateResult.Errors.ToList();
                }
                rate = rateResult.Value;
            }
            dto.ShippingRate = rate;

            // ================================================================
            // 7. Generate Tracking Number
            // ================================================================
            var trackingResult = await _trackingNumberCalculator.GenerateTrackingNumber();
            if (trackingResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return trackingResult.FirstError!;
            }
            dto.TrackingNumber = trackingResult.Value;

            // ================================================================
            // 8. Create Shipment Entity with Navigation Properties
            // ================================================================
            var shipment = _mapper.Map<CreateShipmentDto, TbShipment>(dto);
            shipment.Sender = sender;   // ✅ Navigation property (FK will be auto-set)
            shipment.Receiver = receiver; // ✅ Navigation property (FK will be auto-set)

            // The DTO already has SenderId/ReceiverId, but we don't use them
            // EF Core will automatically set the correct FK values when saving

            // ================================================================
            // 9. Add Shipment to DbContext (NO SAVE yet)
            // ================================================================
            var result = await AddAsync(dto);  // Your BaseService.AddAsync
            if (result.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return result.Value.Id;
            }

            // ================================================================
            // 10. Update Subscription Usage (if used)
            // ================================================================
            if (consumedSubscription is not null)
            {
                var updateResult = await _rateCalculator.ApplySubscriptionUsageAsync(
                    consumedSubscription,
                    dto.Weight,
                    distance
                );
                if (updateResult.IsFailure)
                {
                    await _shippingContext.Database.RollbackTransactionAsync();
                    return updateResult.Errors.ToList();
                }
            }

            // ================================================================
            // 11. SAVE EVERYTHING IN ONE SHOT + COMMIT
            // ================================================================
            await _unitOfWork.CommitTransactionAsync();

            // ✅ Now all entities have real IDs from the database
            // result.Value contains the Shipment ID

            return result.Value.Id;
        }
        catch
        {
            await _shippingContext.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<Result<Guid>> CreateShipmentv1(CreateShipmentDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = await _userService.GetLoggedInUserAsync();


            if (string.IsNullOrEmpty(userId.ToString()))
                return Error.Unauthorized("User.NotLoggedIn", "User must be logged in to create a shipment.");

            // 1. إنشاء الـ Sender
            var senderResult = await _userSenderService.AddAsync(dto.Sender);
            if (senderResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return senderResult.Errors.ToList();
            }
            //dto.SenderId = senderResult.Value;

            // 2. إنشاء الـ Receiver
            var receiverResult = await _userReceiverService.AddAsync(dto.Receiver);
            if (receiverResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return receiverResult.Errors.ToList();
            }
            //dto.ReceiverId = receiverResult.Value;

            //if (dto.SenderId == dto.ReceiverId)
            //{
            //    await _shippingContext.Database.RollbackTransactionAsync();
            //    return Error.Validation("SameSenderReceiver", "Sender and receiver cannot be the same person.");
            //}

            // 3. باقي المراجع (ShippingType, PaymentMethod, Subscription, Packaging, Reference)
            var referencesResult = await ValidateShipmentReferencesAsync(dto);
            if (referencesResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return referencesResult.Errors.ToList();
            }
            var shippingType = referencesResult.Value;

            // 4. Entities كاملة بتاعة الـ Sender/Receiver (للحسابات - Rate/Distance)
            var sender = await _userSenderService.GetByIdAsync(dto.SenderId);
            var receiver = await _userReceiverService.GetByIdAsync(dto.ReceiverId);

            var distance = await _distanceService.GetDistanceBetweenCitiesAsync(sender.Value.CityId, receiver.Value.CityId);

            // 5. Subscription (اختياري)
            TbUserSubscription? consumedSubscription = null;

            if (dto.UserSubscriptionId is { } subId && subId != Guid.Empty)
            {
                var subscriptionResult = await _rateCalculator.TryConsumeFromSubscriptionAsync(subId, dto, distance);

                if (subscriptionResult.IsSuccess)
                    consumedSubscription = subscriptionResult.Value;
                else
                    dto.UserSubscriptionId = null;
            }

            // 6. حساب السعر
            decimal rate;
            if (consumedSubscription is not null)
            {
                rate = 0m;
            }
            else
            {
                var rateResult = await _rateCalculator.CalculateStandardRateAsync(dto, shippingType, distance);
                if (rateResult.IsFailure)
                {
                    await _shippingContext.Database.RollbackTransactionAsync();
                    return rateResult.Errors.ToList();
                }
                rate = rateResult.Value;
            }
            dto.ShippingRate = rate;

            // 7. رقم التتبع
            var trackingResult = await _trackingNumberCalculator.GenerateTrackingNumber();
            if (trackingResult.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return trackingResult.FirstError!;
            }
            dto.TrackingNumber = trackingResult.Value;

            // 8. إنشاء الشحنة نفسها - AddAsync القياسية، هتاخد SenderId/ReceiverId من الـ dto تلقائي
            var result = await AddAsync(dto);
            if (result.IsFailure)
            {
                await _shippingContext.Database.RollbackTransactionAsync();
                return result.Value.Id;
            }

            // 9. تحديث استهلاك الاشتراك لو اتستخدم
            if (consumedSubscription is not null)
            {
                var updateResult = await _rateCalculator.ApplySubscriptionUsageAsync(consumedSubscription, dto.Weight, distance);
                if (updateResult.IsFailure)
                {
                    await _shippingContext.Database.RollbackTransactionAsync();
                    return updateResult.Errors.ToList();
                }
            }

            await _unitOfWork.CommitTransactionAsync();
            return result.Value.Id;
        }
        catch
        {
            await _shippingContext.Database.RollbackTransactionAsync();
            throw;
        }
    }
    
    #region Private Methods


    private async Task<Result<TbShippingType>> ValidateShipmentReferencesAsync(CreateShipmentDto dto)
    {
        var errors = new List<Error>();
        TbShippingType? shippingType = null;

        if (dto.ShippingTypeId == Guid.Empty)
            errors.Add(Error.Validation("ShippingType.Required", "Shipping type is required."));
        else
        {
            shippingType = await _shippingTypeRepository.GetByIdAsync(dto.ShippingTypeId);
            if (shippingType is null)
                errors.Add(Error.NotFound("ShippingType.NotFound", "Shipping type was not found."));
        }

        if (dto.PaymentMethodId.HasValue && dto.PaymentMethodId.Value != Guid.Empty)
        {
            var paymentMethod = await _paymentMethodService.GetByIdAsync(dto.PaymentMethodId.Value);
            if (paymentMethod.IsFailure || paymentMethod.Value is null)
                errors.Add(Error.NotFound("PaymentMethod.NotFound", "Payment method was not found."));
        }

        if (dto.UserSubscriptionId.HasValue && dto.UserSubscriptionId.Value != Guid.Empty)
        {
            var subscription = await _userSubscriptionService.GetByIdAsync(dto.UserSubscriptionId.Value);
            if (subscription.IsFailure || subscription.Value is null)
                errors.Add(Error.NotFound("UserSubscription.NotFound", "User subscription was not found."));
        }

        if (dto.ShippingPackagingId.HasValue && dto.ShippingPackagingId.Value != Guid.Empty)
        {
            var packaging = await _shippingPackagingService.GetByIdAsync(dto.ShippingPackagingId.Value);
            if (packaging.IsFailure || packaging.Value is null)
                errors.Add(Error.NotFound("ShippingPackaging.NotFound", "Shipping packaging was not found."));
        }

        if (dto.ReferenceId.HasValue && dto.ReferenceId.Value != Guid.Empty)
        {
            var referenceShipment = await GetByIdAsync(dto.ReferenceId.Value);
            if (referenceShipment.IsFailure || referenceShipment.Value is null)
                errors.Add(Error.NotFound("ReferenceShipment.NotFound", "Reference shipment was not found."));
        }

        if (errors.Any())
            return Result<TbShippingType>.Failure(errors.ToArray());

        return Result<TbShippingType>.Success(shippingType!);
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