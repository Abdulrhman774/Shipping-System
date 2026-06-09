using Domain.Entities;
using BL.DTOs.Carrier;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class CarrierService 
    : BaseService<TbCarrier, CarrierDto, CreateCarrierDto, UpdateCarrierDto>, ICarrierService
{
    public CarrierService(IGenericRepository<TbCarrier> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}
