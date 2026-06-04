using Domain.Entities;
using BL.DTOs.UserSubscription;

namespace BL.Contracts.IServices;

public interface IUserSubscriptionService 
    : IBaseService<TbUserSubscription, UserSubscriptionDto, CreateUserSubscriptionDto, UpdateUserSubscriptionDto> { }
