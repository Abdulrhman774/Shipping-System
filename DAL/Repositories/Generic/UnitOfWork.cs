using DAL.Context;
using DAL.Contracts;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Data;

namespace DAL.Repositories.Generic;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShippingDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();
    private readonly ConcurrentDictionary<Type, object> _viewRepositories = new();
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;
    private int _transactionCount = 0;


    public UnitOfWork(ShippingDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = loggerFactory.CreateLogger<UnitOfWork>();
    }



    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transactionCount == 0)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("Physical transaction started.");
        }
        else
        {
            _logger.LogDebug("Nested transaction requested. Count: {Count}", _transactionCount);
        }
        _transactionCount++;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transactionCount <= 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        if (_transactionCount > 1)
        {
            _transactionCount--;
            return;
        }


        // Last transaction level, commit the transaction and save changes
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            _transactionCount = 0;
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;


        try
        {
            await _transaction.RollbackAsync(cancellationToken);
            _logger.LogWarning("Transaction rolled back.");
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
            _transactionCount = 0;
        }
    }

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        return (IGenericRepository<T>)_repositories.GetOrAdd(
            typeof(T),
            _ => new GenericRepository<T>(_context, _loggerFactory.CreateLogger<GenericRepository<T>>()));
    }

    public IViewRepository<TView> ViewRepository<TView>() where TView : class
    {
        return (IViewRepository<TView>)_viewRepositories.GetOrAdd(
            typeof(TView),
            _ => new ViewRepository<TView>(_context, _loggerFactory.CreateLogger<ViewRepository<TView>>()));
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);


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



    /// <summary>
    /// Executes the specified operation within a database transaction.
    /// If the operation completes successfully, the transaction is committed;
    /// otherwise, it is rolled back.
    /// </summary>
    public async Task<TResult> ExecuteWithRetryAsync<TResult>(
    Func<Task<TResult>> operation,
    CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
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
        });
    }


    /// <summary>
    /// Executes the specified operation within a database transaction.
    /// If the operation completes successfully, the transaction is committed;
    /// otherwise, it is rolled back.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation result.</typeparam>
    /// <returns>The result returned by the operation.</returns>
    public async Task ExecuteWithRetryAsync(
    Func<Task> operation,
    CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
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
        });
    }


    /// <summary>
    /// Clears all tracked entities from the current DbContext.
    /// Useful after bulk operations or when you want to release memory
    /// and prevent stale tracked entities from affecting subsequent operations.
    /// </summary>
    public void ResetChangeTracker()
    {
        _context.ChangeTracker.Clear();
        _logger.LogInformation("Change tracker cleared.");
    }



    /// <summary>
    /// Returns the number of entities currently being tracked by the DbContext.
    /// Primarily intended for diagnostics, debugging, and performance monitoring.
    /// </summary>
    public int GetTrackedEntitiesCount()
    {
        return _context.ChangeTracker.Entries().Count();
    }


    /// <summary>
    /// Enables or disables Entity Framework Core's automatic change detection.
    /// Disabling it can improve performance during large batch operations,
    /// but changes must be detected manually before saving if required.
    /// </summary>
    public void SetAutoDetectChanges(bool enabled)
    {
        _context.ChangeTracker.AutoDetectChangesEnabled = enabled;
    }



    /// <summary>
    /// Opens the underlying database connection if it is not already open.
    /// Useful for scenarios that execute multiple operations over a single
    /// connection or require manual connection management.
    /// </summary>
    public async Task OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }
    }


    /// <summary>
    /// Closes the underlying database connection if it is currently open.
    /// Intended for use when the connection has been explicitly opened
    /// through this Unit of Work.
    /// </summary>
    public async Task CloseConnectionAsync()
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State == ConnectionState.Open)
        {
            await connection.CloseAsync();
        }
    }


    /// <summary>
    /// Determines whether the underlying database connection is currently open.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the connection is open; otherwise, <c>false</c>.
    /// </returns>
    public bool IsConnectionOpen()
    {
        return _context.Database.GetDbConnection().State == ConnectionState.Open;
    }


    /// <summary>
    /// Indicates whether there is an active database transaction currently
    /// managed by this Unit of Work.
    /// </summary>
    public bool HasActiveTransaction => _transaction is not null && _transactionCount > 0;

}

