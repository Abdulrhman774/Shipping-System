using Domain.Entities;
using BL.DTOs.City;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class CityService 
    : BaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto>, ICityService
{
    public CityService(IGenericRepository<TbCity> repository, IMapper mapper) 
        : base(repository, mapper) { }
}
