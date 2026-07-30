using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    IViewRepository<TView> ViewRepository<TView>() where TView : class;
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


    // Executes the provided operation within a transaction.
    // If the operation completes successfully, the transaction is committed; otherwise, it is rolled back.
    Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default);



    /// <summary>
    /// Executes the specified operation within a database transaction.
    /// If the operation completes successfully, the transaction is committed;
    /// otherwise, it is rolled back.
    /// </summary>
    Task ExecuteWithRetryAsync(Func<Task> operation, CancellationToken cancellationToken = default);


    /// <summary>
    /// Executes the specified operation within a database transaction.
    /// If the operation completes successfully, the transaction is committed;
    /// otherwise, it is rolled back.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation result.</typeparam>
    /// <returns>The result returned by the operation.</returns>
    Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears all tracked entities from the current DbContext.
    /// </summary>
    void ResetChangeTracker();

    /// <summary>
    /// Returns the number of entities currently tracked by the DbContext.
    /// </summary>
    int GetTrackedEntitiesCount();

    /// <summary>
    /// Enables or disables Entity Framework Core automatic change detection.
    /// </summary>
    /// <param name="enabled">
    /// <c>true</c> to enable automatic change detection; otherwise, <c>false</c>.
    /// </param>
    void SetAutoDetectChanges(bool enabled);

    /// <summary>
    /// Opens the underlying database connection if it is not already open.
    /// </summary>
    Task OpenConnectionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the underlying database connection if it is currently open.
    /// </summary>
    Task CloseConnectionAsync();

    /// <summary>
    /// Determines whether the underlying database connection is currently open.
    /// </summary>
    bool IsConnectionOpen();

    /// <summary>
    /// Gets a value indicating whether there is an active database transaction.
    /// </summary>
    bool HasActiveTransaction { get; }
}
