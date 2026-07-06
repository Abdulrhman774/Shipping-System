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
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


    // Executes the provided operation within a transaction.
    // If the operation completes successfully, the transaction is committed; otherwise, it is rolled back.
    Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default);
}
