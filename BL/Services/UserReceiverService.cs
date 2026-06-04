using Domain.Entities;
using BL.DTOs.UserReceiver;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class UserReceiverService 
    : BaseService<TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto>, IUserReceiverService
{
    public UserReceiverService(IGenericRepository<TbUserReceiver> repository, IMapper mapper) 
        : base(repository, mapper) { }
}