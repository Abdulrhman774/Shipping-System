using Domain.Entities;
using BL.DTOs.Country;

namespace BL.Contract.IServices;

public interface ICountryService 
    : IBaseService<TbCountry, CountryDto, CreateCountryDto, UpdateCountryDto> { }
