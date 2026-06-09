using BL.DTOs.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contract.IvwServices;

public interface IVwCitiesCountriesService
{
    Task<List<VwCitiesCountriesDto>> GetAllAsync();
}
