using Domain.Entities;
using BL.DTOs.Country;
using BL.Contract.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class CountryService 
    : BaseService<TbCountry, CountryDto, CreateCountryDto, UpdateCountryDto>, ICountryService
{
    public CountryService(IGenericRepository<TbCountry> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService)
    {

    }



}