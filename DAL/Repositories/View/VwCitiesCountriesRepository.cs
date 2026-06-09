using DAL.Context;
using DAL.Contracts;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Domain.Entities.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.View;

public class VwCitiesCountriesRepository : ViewRepository<VwCitiesCountries>, IViewRepository<VwCitiesCountries>
{
    public VwCitiesCountriesRepository(ShippingDbContext context, ILogger<ViewRepository<VwCitiesCountries>> logger) : base(context, logger)
    { }
}
