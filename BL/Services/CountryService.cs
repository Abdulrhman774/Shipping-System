using Domain.Entities;
using BL.DTOs.Country;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class CountryService 
    : BaseService<TbCountry, CountryDto, CreateCountryDto, UpdateCountryDto>, ICountryService
{
    public CountryService(IGenericRepository<TbCountry> repository, IMapper mapper) 
        : base(repository, mapper) { }
}