using BL.Contract.IServices;
using BL.DTOs.UserReceiver;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class UserReceiverService: SharedSenderReceiverService<TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto>, IUserReceiverService
{
    public UserReceiverService(
        IUnitOfWork unitOfWork,
        IBaseMapper mapper,
        IUserService userService,
        ICityService cityService)
        : base(unitOfWork, mapper, userService, cityService)
    {
    }
}