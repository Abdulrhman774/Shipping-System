using Domain.Entities;
using BL.DTOs.City;

namespace BL.Contract.IServices;

public interface ICityService 
    : IBaseService<TbCity, CityDto, CreateCityDto, UpdateCityDto> { }
