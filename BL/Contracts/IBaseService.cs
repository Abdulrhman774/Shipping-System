using Domain.Shared;
namespace BL.Contracts;

public interface IBaseService<T, TDto, TCreateDto, TUpdateDto> where T : BaseEntity
{
    Task<List<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(Guid id);
    Task<bool> AddAsync(TCreateDto dto, Guid createdBy);
    Task<bool> UpdateAsync(Guid id, TUpdateDto dto, Guid updatedBy);
    Task<bool> DeleteAsync(Guid id, Guid updatedBy);
    Task<bool> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active);
}
