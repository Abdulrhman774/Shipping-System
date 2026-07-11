// BL/Services/UserReceiverService.cs

using Domain.Entities;
using BL.DTOs.UserReceiver;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;
using BL.Common.Results;

namespace BL.Services;

public class UserReceiverService
    : BaseService<TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto>, IUserReceiverService
{
    private readonly ICityService _cityService;

    public UserReceiverService(
        IGenericRepository<TbUserReceiver> repository,
        IMapper mapper,
        IUserService userService,
        ICityService cityService)
        : base(repository, mapper, userService)
    {
        _cityService = cityService;
    }

    // Override CreateAsync with the same logic
    public override async Task<Result<Guid>> CreateAsync(CreateUserReceiverDto dto)
    {
        // 1. Validate City exists
        var cityExists = await _cityService.GetByIdAsync(dto.CityId);
        if (cityExists.IsFailure)
            return Error.NotFound("City.NotFound", "The specified city does not exist.");

        // 2. Check for duplicate Email
        var emailExists = await _repository.ExistsAsync(x => x.Email == dto.Email);
        if (emailExists)
            return Error.Conflict("Email.AlreadyExists", "A receiver with this email already exists.");

        // 3. Check for duplicate Phone
        var phoneExists = await _repository.ExistsAsync(x => x.Phone == dto.Phone);
        if (phoneExists)
            return Error.Conflict("Phone.AlreadyExists", "A receiver with this phone number already exists.");

        // 4. If this is set as default, ensure no other default exists for this user
        if (dto.IsDefaultAddress && !string.IsNullOrEmpty(dto.UserId))
        {
            await RemoveOtherDefaultAddresses(dto.UserId);
        }

        // 5. Proceed with base creation
        return await base.CreateAsync(dto);
    }

    // Override UpdateAsync to handle email/phone changes and default address logic
    public override async Task<Result> UpdateAsync(Guid id, UpdateUserReceiverDto dto)
    {
        // 1. Get existing entity
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return Error.NotFound("Receiver.NotFound", "Receiver was not found.");

        // 2. Validate City exists
        var cityExists = await _cityService.GetByIdAsync(dto.CityId);
        if (cityExists.IsFailure)
            return Error.NotFound("City.NotFound", "The specified city does not exist.");

        // 3. Check for duplicate Email (excluding current entity)
        var emailExists = await _repository.ExistsAsync(x => x.Email == dto.Email && x.Id != id);
        if (emailExists)
            return Error.Conflict("Email.AlreadyExists", "A receiver with this email already exists.");

        // 4. Check for duplicate Phone (excluding current entity)
        var phoneExists = await _repository.ExistsAsync(x => x.Phone == dto.Phone && x.Id != id);
        if (phoneExists)
            return Error.Conflict("Phone.AlreadyExists", "A receiver with this phone number already exists.");

        // 5. If this is set as default, ensure no other default exists for this user
        if (dto.IsDefaultAddress && !string.IsNullOrEmpty(dto.UserId))
        {
            await RemoveOtherDefaultAddresses(dto.UserId, id);
        }

        // 6. Proceed with base update
        return await base.UpdateAsync(id, dto);
    }

    #region Private Helper Methods

    /// <summary>
    /// Removes all other default addresses for a user.
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="excludeId">Optional: Exclude a specific address from being updated</param>
    private async Task RemoveOtherDefaultAddresses(string userId, Guid? excludeId = null)
    {
        var allReceivers = await _repository.GetListAsync(x => x.UserId == userId);

        foreach (var receiver in allReceivers)
        {
            if (receiver.IsDefaultAddress && (excludeId is null || receiver.Id != excludeId))
            {
                receiver.IsDefaultAddress = false;
                await _repository.UpdateAsync(receiver.Id, receiver);
            }
        }
    }

    #endregion
}