using BL.Contract.IServices;
using BL.DTOs.UserSender;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class UserSenderService
    : SharedSenderReceiverService<
        TbUserSender,
        UserSenderDto,
        CreateUserSenderDto,
        UpdateUserSenderDto>,
      IUserSenderService
{
    public UserSenderService(
        IUnitOfWork unitOfWork,
        IBaseMapper mapper,
        IUserService userService,
        ICityService cityService)
        : base(unitOfWork, mapper, userService, cityService)
    {
    }
}