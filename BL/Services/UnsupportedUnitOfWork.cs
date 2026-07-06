using DAL.Contracts;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

/// <summary>
/// Null Object implementation used when a Service is constructed without a real IUnitOfWork
/// (i.e. via the Repository-only constructor). Any attempt to use transactional
/// features in this state throws a clear, descriptive exception instead of a NullReferenceException.
/// </summary>
internal sealed class UnsupportedUnitOfWork : IUnitOfWork
{
    private const string Message =
        "This service was constructed with a Repository directly (no IUnitOfWork). " +
        "Transactional operations are not supported in this mode.";

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(Message);

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(Message);

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(Message);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(Message);

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity =>
        throw new InvalidOperationException(Message);

    public Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException(Message);

    public Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<Task<TResult>> operation, CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(Message);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}