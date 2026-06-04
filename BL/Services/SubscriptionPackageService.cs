using Domain.Entities;
using BL.DTOs.SubscriptionPackage;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class SubscriptionPackageService 
    : BaseService<TbSubscriptionPackage, SubscriptionPackageDto, CreateSubscriptionPackageDto, UpdateSubscriptionPackageDto>, ISubscriptionPackageService
{
    public SubscriptionPackageService(IGenericRepository<TbSubscriptionPackage> repository, IMapper mapper) 
        : base(repository, mapper) { }
}