using Domain.Entities;
using BL.DTOs.UserReceiver;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class UserReceiverService 
    : BaseService<TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto>, IUserReceiverService
{
    public UserReceiverService(IGenericRepository<TbUserReceiver> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}