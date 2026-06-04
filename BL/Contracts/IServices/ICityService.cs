using Domain.Entities;
using BL.DTOs.City;

namespace BL.Contracts.IServices;

public interface ICityService 
    : IBaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto> { }
