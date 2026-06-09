using Domain.Entities;
using BL.DTOs.UserSender;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class UserSenderService 
    : BaseService<TbUserSender, UserSenderDto, CreateUserSenderDto, UpdateUserSenderDto>, IUserSenderService
{
    public UserSenderService(IGenericRepository<TbUserSender> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}