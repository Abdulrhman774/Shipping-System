using BL.DTOs.City;
using BL.DTOs.Country;
using Domain.Entities;

namespace BL.Contract.IServices;

public interface ICityService 
    : IBaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto> {
    Task<List<CityDto>> GetAllByCountryId(Guid CountryId);
}
