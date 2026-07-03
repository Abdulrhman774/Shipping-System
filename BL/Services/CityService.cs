using Domain.Entities;
using BL.DTOs.City;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;
using BL.DTOs.Country;

namespace BL.Services;

public class CityService 
    : BaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto>, ICityService
{
    public CityService(IGenericRepository<TbCity> repository, IMapper mapper, IUserService userService) 
        : base(repository, mapper, userService) { }

    public async Task<List<CityDto>> GetAllByCountryId(Guid CountryId)
    {
        var cities = await _repository.GetListAsync(e => e.CountryId == CountryId);

        return _mapper.MapList<TbCity, CityDto>(cities);
    }
}
