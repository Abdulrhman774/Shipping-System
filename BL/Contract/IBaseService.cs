using BL.Common;
using BL.Common.Results;
using Domain.Shared;
using System.Linq.Expressions;
namespace BL.Contract;

public interface IBaseService<T, TDto, TCreateDto, TUpdateDto> where T : BaseEntity
{
    Task<Result<IEnumerable<TDto>>> GetAllAsync();
    Task<Result<TDto>> GetByIdAsync(Guid id);
    Task<Result<T>> AddAsync(TCreateDto dto, bool? autoSave = null);
    Task<Result> UpdateAsync(Guid id, TUpdateDto dto, bool? autoSave = null);
    Task<Result> DeleteAsync(Guid id, bool? autoSave = null);
    Task<Result> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active, bool? autoSave = null);
    Task<Result<PagedResult<TDto>>> GetPagedAsync(int pageNumber, int pageSize);
    Task<Result<PagedResult<TDto>>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter);
}
