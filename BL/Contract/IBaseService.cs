using BL.Common.Results;
using Domain.Shared;
namespace BL.Contract;

public interface IBaseService<T, TDto, TCreateDto, TUpdateDto> where T : BaseEntity
{
    Task<Result<IEnumerable<TDto>>> GetAllAsync();
    Task<Result<TDto>> GetByIdAsync(Guid id);
    Task<Result> AddAsync(TCreateDto dto);
    Task<Result<Guid>> CreateAsync(TCreateDto dto);
    Task<Result> UpdateAsync(Guid id, TUpdateDto dto);
    Task<Result> DeleteAsync(Guid id);
    Task<Result> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active);
}
