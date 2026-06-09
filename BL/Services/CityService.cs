using Domain.Entities;
using BL.DTOs.City;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class CityService 
    : BaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto>, ICityService
{
    public CityService(IGenericRepository<TbCity> repository, IMapper mapper, IUserService userService) 
        : base(repository, mapper, userService) { }
}
