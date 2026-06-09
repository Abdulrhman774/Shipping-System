using Domain.Entities;
using BL.DTOs.UserSubscription;

namespace BL.Contract.IServices;

public interface IUserSubscriptionService 
    : IBaseService<TbUserSubscription, UserSubscriptionDto, CreateUserSubscriptionDto, UpdateUserSubscriptionDto> { }
