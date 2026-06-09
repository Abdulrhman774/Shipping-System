using BL.Contract.IvwServices;
using BL.DTOs.Views;
using BL.Mapping;
using DAL.Contracts;
using DAL.Repositories.View;
using Domain.Entities.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BL.Services.vwServices
{
    public class VwCitiesCountriesService : IVwCitiesCountriesService
    {
        private readonly IViewRepository<VwCitiesCountries> _repository;
        private readonly IMapper _mapper;
        public VwCitiesCountriesService(IViewRepository<VwCitiesCountries> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<VwCitiesCountriesDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return _mapper.MapList<VwCitiesCountries, VwCitiesCountriesDto>(data);
        }
    }
}
