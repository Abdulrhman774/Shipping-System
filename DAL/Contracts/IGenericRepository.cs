using Domain.Shared;

namespace DAL.Contracts;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(Guid id, T entity);
    Task<bool> DeleteAsync(Guid id, Guid DeletedBy);
    Task<bool> ChangeStatusAsync(Guid id, Guid updatedBy, enEntityState status = enEntityState.Active);
}