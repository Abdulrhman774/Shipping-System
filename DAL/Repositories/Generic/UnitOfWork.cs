using DAL.Context;
using DAL.Contracts;
using Domain.Shared;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Generic;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShippingDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ShippingDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<UnitOfWork>();
    }



    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException(
                "A transaction is already in progress. Commit or rollback it before starting a new one.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken); // هي نفسها بتعمل Dispose + null
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        return (IGenericRepository<T>)_repositories.GetOrAdd(
            typeof(T),
            _ => new GenericRepository<T>(_context, _loggerFactory.CreateLogger<GenericRepository<T>>()));
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);


    // Safety net to ensure that if the UnitOfWork is disposed while a transaction is still open,
    // it will log a warning and roll back the transaction to prevent any uncommitted changes from being lost silently.


    /// <summary>
    /// Safety net to ensure that if the UnitOfWork is disposed while a transaction is still open,
    /// it will log a warning and roll back the transaction to prevent any uncommitted changes from being lost silently.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            _logger.LogWarning(
                "UnitOfWork disposed while a transaction was still open (never committed or rolled back). " +
                "The database will implicitly roll it back, and any pending changes will be lost.");

            await _transaction.DisposeAsync();
        }

        await _context.DisposeAsync();
    }


    /// <summary>
    /// For APIs that don't need to manually call BeginTransactionAsync, CommitAsync, or RollbackAsync.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);
        try
        {
            await operation();
            await CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }


    /// <summary>
    /// For APIs that don't need to manually call BeginTransactionAsync, CommitAsync, or RollbackAsync.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="operation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result of the operation</returns>
    public async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<Task<TResult>> operation, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation();
            await CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }



}

