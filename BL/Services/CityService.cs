using Domain.Entities;
using BL.DTOs.City;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class CityService 
    : BaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto>, ICityService
{
    public CityService(IGenericRepository<TbCity> repository, IBaseMapper mapper, IUserService userService, IUnitOfWork unitOfWork) 
        : base(unitOfWork, mapper, userService) { }

    public async Task<List<CityDto>> GetAllByCountryId(Guid CountryId)
    {
        var cities = await _repository.GetListAsync(e => e.CountryId == CountryId);

        return _mapper.MapList<TbCity, CityDto>(cities);
    }
}
