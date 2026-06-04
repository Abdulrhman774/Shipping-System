using Domain.Entities;
using BL.DTOs.Country;

namespace BL.Contracts.IServices;

public interface ICountryService 
    : IBaseService<TbCountry, CountryDto, CreateCountryDto, UpdateCountryDto> { }
