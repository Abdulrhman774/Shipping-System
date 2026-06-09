using Domain.Shared;
namespace BL.Contract;

public interface IBaseService<T, TDto, TCreateDto, TUpdateDto> where T : BaseEntity
{
    Task<List<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(Guid id);
    Task<bool> AddAsync(TCreateDto dto);
    Task<bool> UpdateAsync(Guid id, TUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active);
}
