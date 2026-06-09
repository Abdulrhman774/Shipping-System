using DAL.Context;
using DAL.Contracts;
using DAL.Exceptions;
using Domain.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Generic;

public class ViewRepository<TView> : IViewRepository<TView> where TView : class
{
    private readonly ShippingDbContext _context;
    private readonly DbSet<TView> _dbSet;
    private readonly ILogger<ViewRepository<TView>> _logger;

    public ViewRepository(ShippingDbContext context, ILogger<ViewRepository<TView>> logger)
    {
        _context = context;
        _dbSet = _context.Set<TView>();
        _logger = logger;
    }

    public async Task<IEnumerable<TView>> GetAllAsync()
    {
        try
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all entities of type {EntityType}", typeof(TView).Name);
            throw new DataAccessException($"Error while getting all {typeof(TView).Name}", ex);
        }
    }
}
