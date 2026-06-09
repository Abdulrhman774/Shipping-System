using Domain.Entities;
using BL.DTOs.UserSubscription;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class UserSubscriptionService 
    : BaseService<TbUserSubscription, UserSubscriptionDto, CreateUserSubscriptionDto, UpdateUserSubscriptionDto>, IUserSubscriptionService
{
    public UserSubscriptionService(IGenericRepository<TbUserSubscription> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}