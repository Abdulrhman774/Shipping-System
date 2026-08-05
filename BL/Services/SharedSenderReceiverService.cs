using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.SharedSenderReceiver;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;
using Domain.Shared;

namespace BL.Services;

public abstract class SharedSenderReceiverService<TEntity, TDto, TCreateDto, TUpdateDto>
    : BaseService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : SharedSenderReceiver
    where TCreateDto : SharedCreateSenderReceiverDto
    where TUpdateDto : SharedUpdateSenderReceiverDto
{
    protected readonly ICityService _cityService;

    protected SharedSenderReceiverService(
        IUnitOfWork unitOfWork,
        IBaseMapper mapper,
        IUserService userService,
        ICityService cityService)
        : base(unitOfWork, mapper, userService)
    {
        _cityService = cityService;
    }

    public override async Task<Result<TEntity>> AddAsync(TCreateDto dto, bool? autoSave = null)
    {
        var cityExists = await _cityService.GetByIdAsync(dto.CityId);
        if (cityExists.IsFailure)
            return Error.NotFound("City.NotFound", "The specified city does not exist.");

        if (await _repository.ExistsAsync(x => x.Email == dto.Email))
            return Error.Conflict(
                        "Email.AlreadyExists",
                        $"A {EntityDisplayName} with this email already exists.");

        if (await _repository.ExistsAsync(x => x.Phone == dto.Phone))
            return Error.Conflict(
                        "Phone.AlreadyExists",
                        $"A {EntityDisplayName} with this phone number already exists.");

        if (dto.IsDefaultAddress && !string.IsNullOrWhiteSpace(dto.UserId))
        {
            await RemoveOtherDefaultAddresses(dto.UserId, null);
        }

        return await base.AddAsync(dto, autoSave);
    }

    public override async Task<Result> UpdateAsync(Guid id, TUpdateDto dto, bool? autoSave = null)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return Error.NotFound(
                $"{typeof(TEntity).Name}.NotFound",
                $"{char.ToUpper(EntityDisplayName[0]) + EntityDisplayName[1..]} was not found.");
        }

        var cityExists = await _cityService.GetByIdAsync(dto.CityId);
        if (cityExists.IsFailure)
            return Error.NotFound("City.NotFound", "The specified city does not exist.");

        if (await _repository.ExistsAsync(x => x.Email == dto.Email && x.Id != id))
            return Error.Conflict(
                        "Email.AlreadyExists",
                        $"A {EntityDisplayName} with this email already exists.");

        if (await _repository.ExistsAsync(x => x.Phone == dto.Phone && x.Id != id))
            return Error.Conflict(
                        "Phone.AlreadyExists",
                        $"A {EntityDisplayName} with this phone number already exists.");

        if (dto.IsDefaultAddress && !string.IsNullOrWhiteSpace(dto.UserId))
        {
            await RemoveOtherDefaultAddresses(dto.UserId, id);
        }

        return await base.UpdateAsync(id, dto, autoSave);
    }

    protected async Task RemoveOtherDefaultAddresses(string userId, Guid? excludeId = null)
    {
        var addresses = await _repository.GetListAsync(x => x.UserId == userId, tracking: true);

        foreach (var address in addresses)
        {
            if (address.IsDefaultAddress &&
                (excludeId == null || address.Id != excludeId))
            {
                address.IsDefaultAddress = false;
            }
        }
    }

    protected virtual string EntityDisplayName =>
    typeof(TEntity).Name switch
    {
        nameof(TbUserSender) => "sender",
        nameof(TbUserReceiver) => "receiver",
        _ => "entity"
    };
}