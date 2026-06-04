using Domain.Entities;
using BL.DTOs.SubscriptionPackage;

namespace BL.Contracts.IServices;

public interface ISubscriptionPackageService 
    : IBaseService<TbSubscriptionPackage, SubscriptionPackageDto, CreateSubscriptionPackageDto, UpdateSubscriptionPackageDto> { }
