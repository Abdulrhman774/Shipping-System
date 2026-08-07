using Domain.Shared;
using System.Linq.Expressions;

namespace DAL.Contracts;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, bool tracking = false, CancellationToken cancellationToken = default);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken cancellationToken = default);
    IQueryable<T> GetQueryable(bool tracking = false);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(T entity, bool AutoSave = false, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, T entity, bool AutoSave = false, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, Action<T> updateAction, bool AutoSave = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, Guid deletedBy, bool AutoSave = false, CancellationToken cancellationToken = default);
    Task<bool> ChangeStatusAsync(Guid id, Guid updatedBy, enEntityState status = enEntityState.Active, bool AutoSave = false, CancellationToken cancellationToken = default);

    Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool tracking = false,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}