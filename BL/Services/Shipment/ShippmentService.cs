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
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BL.Services.Shipment;

public class ShipmentService
    : BaseService<TbShipment, ShipmentDto, CreateShipmentDto, UpdateShipmentDto>, IShipmentService
{
    #region Private Fields
    private readonly IGenericRepository<TbCarrier> _carrierRepository;
    private readonly ITrackingNumberCalculator _trackingNumberCalculator;
    private readonly IRateCalculator _rateCalculator;
    private readonly IDistanceService _distanceService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IUserSubscriptionService _userSubscriptionService;
    private readonly IShippingPackagingService _shippingPackagingService;
    private readonly IUserSenderService _userSenderService;
    private readonly IUserReceiverService _userReceiverService;
    private readonly IGenericRepository<TbShippingType> _shippingTypeRepository;
    #endregion

    public ShipmentService(
        ShippingDbContext shippingContext,
        IBaseMapper mapper,
        IUserService userService,
        ITrackingNumberCalculator trackingNumberCalculator,
        IRateCalculator rateCalculator,
        IDistanceService distanceService,
        IUnitOfWork unitOfWork,
        IUserSenderService userSenderservice,
        IUserReceiverService userReceiverService,
        IPaymentMethodService paymentMethodService,
        IUserSubscriptionService userSubscriptionService,
        IShippingPackagingService shippingPackagingService,
        IGenericRepository<TbCarrier> carrierRepository)
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

        // ✅ Get repository from UnitOfWork
        _shippingTypeRepository = unitOfWork.Repository<TbShippingType>();
        _carrierRepository = carrierRepository;
    }



    // ================================================================
    // ✅ CREATE SHIPMENT
    // ================================================================
    public async Task<Result<ShipmentDto>> CreateShipment(CreateShipmentRequestDto requestDto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = await _userService.GetLoggedInUserAsync();
            if (string.IsNullOrEmpty(userId.ToString()))
                return Error.Unauthorized("User.NotLoggedIn", "User must be logged in to create a shipment.");

            // ================================================================
            // 1. Create Sender (Validation + Tracking, NO SAVE)
            // ================================================================
            var senderResult = await _userSenderService.AddAsync(requestDto.SenderDto);
            if (senderResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return senderResult.Errors.ToList();
            }
            var sender = senderResult.Value;

            // ================================================================
            // 2. Create Receiver (Validation + Tracking, NO SAVE)
            // ================================================================
            var receiverResult = await _userReceiverService.AddAsync(requestDto.ReceiverDto);
            if (receiverResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return receiverResult.Errors.ToList();
            }
            var receiver = receiverResult.Value;

            // ================================================================
            // 3. Validate Sender != Receiver
            // ================================================================
            if (sender.Id == receiver.Id)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Error.Validation("SameSenderReceiver", "Sender and receiver cannot be the same person.");
            }

            // ================================================================
            // 4. Validate References (ShippingType, PaymentMethod, etc.)
            // ================================================================
            var referencesResult = await ValidateShipmentReferencesAsync(requestDto.ShipmentDto);
            if (referencesResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return referencesResult.Errors.ToList();
            }
            var shippingType = referencesResult.Value;

            // ================================================================
            // 5. Get Distance
            // ================================================================
            var distance = await _distanceService.GetDistanceBetweenCitiesAsync(
                requestDto.SenderDto.CityId,
                requestDto.ReceiverDto.CityId
            );

            // ================================================================
            // 6. Subscription (Optional)
            // ================================================================
            TbUserSubscription? consumedSubscription = null;

            if (requestDto.ShipmentDto.UserSubscriptionId is { } subId && subId != Guid.Empty)
            {
                var subscriptionResult = await _rateCalculator.TryConsumeFromSubscriptionAsync(
                    subId,
                    requestDto.ShipmentDto,
                    distance
                );

                if (subscriptionResult.IsSuccess)
                    consumedSubscription = subscriptionResult.Value;
                else
                    requestDto.ShipmentDto.UserSubscriptionId = null;
            }

            // ================================================================
            // 7. Calculate Rate
            // ================================================================
            decimal rate;
            if (consumedSubscription is not null)
            {
                rate = 0m;
            }
            else
            {
                var rateResult = await _rateCalculator.CalculateStandardRateAsync(
                    requestDto.ShipmentDto,
                    shippingType,
                    distance
                );
                if (rateResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return rateResult.Errors.ToList();
                }
                rate = rateResult.Value;
            }
            requestDto.ShipmentDto.ShippingRate = rate;

            // ================================================================
            // 8. Generate Tracking Number
            // ================================================================
            var trackingResult = await _trackingNumberCalculator.GenerateTrackingNumber();
            if (trackingResult.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return trackingResult.FirstError!;
            }
            requestDto.ShipmentDto.TrackingNumber = trackingResult.Value;

            // ================================================================
            // 9. Create Shipment (NO SAVE - just tracking)
            // ================================================================
            // ✅ BaseService.AddAsync يتولى تعيين CreatedBy, CreatedDate, CurrentState
            var result = await AddAsync(requestDto.ShipmentDto);
            if (result.IsFailure)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return result.Errors.ToList();
            }

            var shipment = result.Value; // ✅ Entity متتبعة مع CreatedBy, CreatedDate, CurrentState

            // ✅ ربط Sender و Receiver (Navigation Properties)
            shipment.Sender = sender;
            shipment.Receiver = receiver;

            shipment.Status = enShipmentStatus.Created;
            shipment.StatusLastUpdatedAt = DateTime.UtcNow;

            var history = new TbShipmentStatusHistory
            {
                ShipmentId = shipment.Id,
                Status = enShipmentStatus.Created,
                Note = "Shipment created"
            };


            await _unitOfWork.Repository<TbShipmentStatusHistory>().CreateAsync(history, AutoSave: false);

            // ================================================================
            // 10. Update Subscription Usage (if used)
            // ================================================================
            if (consumedSubscription is not null)
            {
                var updateResult = await _rateCalculator.ApplySubscriptionUsageAsync(
                    consumedSubscription,
                    requestDto.ShipmentDto.Weight,
                    distance
                );
                if (updateResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return updateResult.Errors.ToList();
                }
            }

            // ================================================================
            // 11. SAVE EVERYTHING IN ONE SHOT + COMMIT
            // ================================================================
            await _unitOfWork.CommitTransactionAsync();

            // ================================================================
            // 12. Return the created shipment as DTO
            // ================================================================
            var shipmentDto = _mapper.Map<TbShipment, ShipmentDto>(shipment);
            return Result<ShipmentDto>.Success(shipmentDto);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }


    // ================================================================
    // ✅ UPDATE SHIPMENT (OVERRIDE)
    // ================================================================
    public async Task<Result> UpdateShipment(Guid id, UpdateShipmentRequestDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. جلب الشحنة الحالية
            var existingShipment = await _unitOfWork.Repository<TbShipment>()
                        .GetQueryable()
                        .Include(s => s.Sender)
                        .Include(s => s.Receiver)
                        .FirstOrDefaultAsync(s => s.Id == id);
            
            if (existingShipment is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Error.NotFound("Shipment.NotFound", "Shipment was not found.");
            }

            // 2. التحقق من أن المستخدم الحالي هو منشئ الشحنة
            var userId = await _userService.GetLoggedInUserAsync();
            if (existingShipment.CreatedBy != userId)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Error.Unauthorized("Shipment.Unauthorized", "You are not authorized to edit this shipment.");
            }

            // 3. التحقق من أن الشحنة غير مكتملة (قابلة للتعديل)
            if (existingShipment.CurrentState == enEntityState.Inactive)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Error.Validation("Shipment.Inactive", "Cannot edit an inactive shipment.");
            }

            // ✅ جديد: منع التعديل إذا كانت الحالة Dispatched أو Delivered
            if (existingShipment.Status == enShipmentStatus.Shipped || existingShipment.Status == enShipmentStatus.Delivered)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Error.Validation("Shipment.CannotUpdate", "Cannot update shipment because it is already Dispatched or Delivered.");
            }

            // 4. تحديث بيانات Sender
            if (dto.ShipmentDto.SenderId != Guid.Empty)
            {
                var senderUpdateResult = await _userSenderService.UpdateAsync(dto.ShipmentDto.SenderId, new UpdateUserSenderDto
                {
                    Name = dto.SenderDto.Name ?? existingShipment.Sender.Name,
                    Email = dto.SenderDto.Email ?? existingShipment.Sender.Email,
                    Phone = dto.SenderDto.Phone ?? existingShipment.Sender.Phone,
                    Address = dto.SenderDto.Address ?? existingShipment.Sender.Address,
                    PostalCode = dto.SenderDto.PostalCode ?? existingShipment.Sender.PostalCode,
                    Contact = dto.SenderDto.Contact ?? existingShipment.Sender.Contact,
                    OtherAddress = dto.SenderDto.OtherAddress ?? existingShipment.Sender.OtherAddress,
                    CityId = (dto.SenderDto.CityId == Guid.Empty) ? existingShipment.Sender.CityId : dto.SenderDto.CityId,
                    IsDefaultAddress = dto.SenderDto.IsDefaultAddress
                });

                if (senderUpdateResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return senderUpdateResult.Errors.ToList();
                }
            }

            // 5. تحديث بيانات Receiver
            if (dto.ShipmentDto.ReceiverId != Guid.Empty)
            {
                var receiverUpdateResult = await _userReceiverService.UpdateAsync(dto.ShipmentDto.ReceiverId, new UpdateUserReceiverDto
                {
                    Name = dto.ReceiverDto.Name ?? existingShipment.Receiver.Name,
                    Email = dto.ReceiverDto.Email ?? existingShipment.Receiver.Email,
                    Phone = dto.ReceiverDto.Phone ?? existingShipment.Receiver.Phone,
                    Address = dto.ReceiverDto.Address ?? existingShipment.Receiver.Address,
                    PostalCode = dto.ReceiverDto.PostalCode ?? existingShipment.Receiver.PostalCode,
                    Contact = dto.ReceiverDto.Contact ?? existingShipment.Receiver.Contact,
                    OtherAddress = dto.ReceiverDto.OtherAddress ?? existingShipment.Receiver.OtherAddress,
                    CityId = (dto.ReceiverDto.CityId == Guid.Empty) ? existingShipment.Receiver.CityId : dto.ReceiverDto.CityId,
                    IsDefaultAddress = dto.ReceiverDto.IsDefaultAddress
                });

                if (receiverUpdateResult.IsFailure)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return receiverUpdateResult.Errors.ToList();
                }
            }

            // 6. تحديث بيانات الشحنة
            existingShipment.ShippingDate = dto.ShipmentDto.ShippingDate;
            existingShipment.DeliveryDate = dto.ShipmentDto.DeliveryDate;
            existingShipment.ShippingTypeId = dto.ShipmentDto.ShippingTypeId;
            existingShipment.ShippingPackagingId = dto.ShipmentDto.ShippingPackagingId;
            existingShipment.Width = dto.ShipmentDto.Width;
            existingShipment.Height = dto.ShipmentDto.Height;
            existingShipment.Weight = dto.ShipmentDto.Weight;
            existingShipment.Length = dto.ShipmentDto.Length;
            existingShipment.PackageValue = dto.ShipmentDto.PackageValue;
            existingShipment.PaymentMethodId = dto.ShipmentDto.PaymentMethodId;
            existingShipment.UserSubscriptionId = dto.ShipmentDto.UserSubscriptionId;
            existingShipment.ReferenceId = dto.ShipmentDto.ReferenceId;
            existingShipment.UpdatedDate = DateTime.UtcNow;
            existingShipment.UpdatedBy = userId;

            // 7. إعادة حساب السعر (لو تغير الوزن أو المسافة)
            var shippingType = await _shippingTypeRepository.GetByIdAsync(dto.ShipmentDto.ShippingTypeId);
            if (shippingType is not null)
            {
                var distance = await _distanceService.GetDistanceBetweenCitiesAsync(
                    existingShipment.Sender.CityId,
                    existingShipment.Receiver.CityId
                );

                var rateResult = await _rateCalculator.CalculateStandardRateAsync(
                    new CreateShipmentDto
                    {
                        Weight = dto.ShipmentDto.Weight,
                        Width = dto.ShipmentDto.Width,
                        Height = dto.ShipmentDto.Height,
                        Length = dto.ShipmentDto.Length,
                        PackageValue = dto.ShipmentDto.PackageValue
                    },
                    shippingType,
                    distance
                );

                if (rateResult.IsSuccess)
                {
                    existingShipment.ShippingRate = rateResult.Value;
                }
            }

            // 8. حفظ التغييرات
            var updated = await _repository.UpdateAsync(id, existingShipment);
            if (!updated)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Error.Unexpected("Shipment.UpdateFailed", "Failed to update shipment.");
            }

            await _unitOfWork.CommitTransactionAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Error.Unexpected("Shipment.UpdateError", $"An error occurred: {ex.Message}");
        }
    }






    // ── Paste this region inside ShipmentService.cs ────────────────────────────
    // Place it after the existing UpdateShipment method, before the closing brace
    // of the class.
    //
    // Also add to the constructor field list (already injected via IUnitOfWork):
    //   _carrierRepository = unitOfWork.Repository<TbCarrier>();
    //
    // And add the backing field anywhere in the #region Private Fields block:
    //   private readonly IGenericRepository<TbCarrier> _carrierRepository;
    // ───────────────────────────────────────────────────────────────────────────

    #region Status Transitions

    // ================================================================
    // APPROVE SHIPMENT  (Created | Returned  →  Approved)
    // ================================================================
    public Task<Result> ApproveShipmentAsync(Guid shipmentId, string? note = null, CancellationToken ct = default)
    {
        return TransitionStatusAsync(
                shipmentId,
                allowedFromStates: [enShipmentStatus.Created, enShipmentStatus.Returned],
                targetStatus: enShipmentStatus.Approved,
                note: note ?? "Shipment approved.",
                ct: ct);
    }

    // ================================================================
    // READY FOR SHIP  (Approved | Created  →  ReadyForShip)
    // ================================================================
    public Task<Result> MarkReadyForShipAsync(Guid shipmentId, string? note = null, CancellationToken ct = default)
    {
        return TransitionStatusAsync(
                shipmentId,
                allowedFromStates: [enShipmentStatus.Approved, enShipmentStatus.Created],
                targetStatus: enShipmentStatus.ReadyForShip,
                note: note ?? "Shipment is ready for shipping.",
                ct: ct);
    }

    // ================================================================
    // SHIPPED  (ReadyForShip | Approved  →  Shipped)
    // ================================================================
    public async Task<Result> MarkShippedAsync(
        Guid shipmentId,
        ShipShipmentDto dto,
        CancellationToken ct = default)
    {
        // 1. Validate carrier exists before opening a transaction
        var carrierExists = await _carrierRepository
            .ExistsAsync(dto.CarrierId, ct);

        if (!carrierExists)
            return Error.NotFound("Carrier.NotFound",
                "The specified carrier does not exist.");

        // 2. Delegate the status transition (will open its own transaction)
        var transitionResult = await TransitionStatusAsync(
            shipmentId,
            allowedFromStates: [enShipmentStatus.ReadyForShip, enShipmentStatus.Approved],
            targetStatus: enShipmentStatus.Shipped,
            note: dto.Note ?? "Shipment dispatched.",
            ct: ct,
            // Extra work executed inside the same transaction, after validation
            // but before commit — perfect for writing the TbShipmentStatus record.
            extraWork: async shipment =>
            {
                var statusRecord = new TbShipmentStatus
                {
                    ShipmentId = shipment.Id,
                    CarrierId = dto.CarrierId,
                    Notes = dto.Note,
                    CreatedBy = shipment.UpdatedBy ?? shipment.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    CurrentState = enEntityState.Active,
                };

                await _unitOfWork
                    .Repository<TbShipmentStatus>()
                    .CreateAsync(statusRecord, AutoSave: false);
            });

        return transitionResult;
    }

    #endregion



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


    /// <summary>
    /// Core state-machine helper.
    /// 
    /// Flow:
    ///   1. Begin transaction
    ///   2. Load shipment (tracking ON so EF tracks the change)
    ///   3. Verify current status is one of <paramref name="allowedFromStates"/>
    ///   4. Update Status + StatusLastUpdatedAt + UpdatedBy + UpdatedDate
    ///   5. Append a TbShipmentStatusHistory row
    ///   6. Run optional <paramref name="extraWork"/> (e.g. write TbShipmentStatus)
    ///   7. SaveChanges + Commit
    /// </summary>
    private async Task<Result> TransitionStatusAsync(
        Guid shipmentId,
        enShipmentStatus[] allowedFromStates,
        enShipmentStatus targetStatus,
        string note,
        CancellationToken ct = default,
        Func<TbShipment, Task>? extraWork = null)
    {
        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            // ── 1. Load ──────────────────────────────────────────────────────
            var shipment = await _repository
                .GetByIdAsync(shipmentId, tracking: true);   // tracking=true so EF detects changes

            if (shipment is null)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                return Error.NotFound(
                    "Shipment.NotFound",
                    $"Shipment '{shipmentId}' was not found.");
            }

            // ── 2. Guard: soft-deleted shipments are unreachable ─────────────
            //    (Global query filter already excludes Deleted rows, but be explicit)
            if (shipment.CurrentState == enEntityState.Deleted)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                return Error.NotFound(
                    "Shipment.NotFound",
                    "Shipment was not found.");
            }

            // ── 3. Validate the state-machine transition ──────────────────────
            if (!allowedFromStates.Contains(shipment.Status))
            {
                await _unitOfWork.RollbackTransactionAsync(ct);

                var allowed = string.Join(" or ",
                    allowedFromStates.Select(s => s.ToString()));

                return Error.Validation(
                    "Shipment.InvalidTransition",
                    $"Cannot transition to '{targetStatus}' from '{shipment.Status}'. " +
                    $"Shipment must be in '{allowed}' status.");
            }

            // ── 4. Identify the actor ─────────────────────────────────────────
            var actorId = await _userService.GetLoggedInUserAsync();

            // ── 5. Apply state change ─────────────────────────────────────────
            shipment.Status = targetStatus;
            shipment.StatusLastUpdatedAt = DateTime.UtcNow;
            shipment.UpdatedBy = actorId;
            shipment.UpdatedDate = DateTime.UtcNow;

            // ── 6. History record ─────────────────────────────────────────────
            var history = new TbShipmentStatusHistory
            {
                ShipmentId = shipment.Id,
                Status = targetStatus,
                Note = note,
                CreatedBy = actorId,
                CreatedDate = DateTime.UtcNow,
                CurrentState = enEntityState.Active,
            };

            await _unitOfWork
                .Repository<TbShipmentStatusHistory>()
                .CreateAsync(history, AutoSave: false);

            // ── 7. Extra work (e.g. TbShipmentStatus for carrier assignment) ──
            if (extraWork is not null)
                await extraWork(shipment);

            // ── 8. Persist everything in one shot ────────────────────────────
            await _unitOfWork.CommitTransactionAsync(ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            return Error.Unexpected(
                "Shipment.TransitionFailed",
                $"An unexpected error occurred during status transition: {ex.Message}");
        }
    }

    #endregion
}