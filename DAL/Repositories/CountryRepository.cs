using DAL.Context;
using DAL.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class CountryRepository : GenericRepository<TbCountry>, ICountryRepository
{
    public CountryRepository(AppDbContext context, ILogger<CountryRepository> logger) : base(context, logger)
    {
    }
}