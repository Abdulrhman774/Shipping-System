using Domain.Entities;
using BL.DTOs.SubscriptionPackage;

namespace BL.Contract.IServices;

public interface ISubscriptionPackageService 
    : IBaseService<TbSubscriptionPackage, SubscriptionPackageDto, CreateSubscriptionPackageDto, UpdateSubscriptionPackageDto> { }
