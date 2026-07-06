using Domain.Shared;
using System.Linq.Expressions;

namespace DAL.Contracts;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<bool> AddAsync(T entity);
    Task<Guid> CreateAsync(T entity);
    Task<bool> UpdateAsync(Guid id, T entity);
    Task<bool> DeleteAsync(Guid id, Guid DeletedBy);
    Task<bool> ChangeStatusAsync(Guid id, Guid updatedBy, enEntityState status = enEntityState.Active);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
}