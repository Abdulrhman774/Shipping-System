using BL.Contract;
using BL.Contract.IServices;
using BL.Mapping;
using DAL.Contracts;
using Domain.Shared;
using System.Security.Principal;

namespace BL.Services;
public class BaseService<T, TDto, TCreateDto, TUpdateDto> : IBaseService<T, TDto, TCreateDto, TUpdateDto> where T : BaseEntity
{
    private readonly IGenericRepository<T> _repository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    public BaseService(IGenericRepository<T> repository, IMapper mapper, IUserService userService)
    {
        _repository = repository;
        _mapper = mapper;
        _userService = userService;
    }
    public async Task<List<TDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return _mapper.MapList<T, TDto>(list);
    }
    public async Task<TDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<T, TDto>(entity);
    }
    public async Task<bool> AddAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<TCreateDto, T>(dto);

        entity.CreatedBy = await _userService.GetLoggedInUserAsync();

        return await _repository.AddAsync(entity);
    }
    public async Task<bool> UpdateAsync(Guid id, TUpdateDto dto)
    {
        var entity = _mapper.Map<TUpdateDto, T>(dto);
        entity.Id = id;
        entity.UpdatedBy = await _userService.GetLoggedInUserAsync();
        return await _repository.UpdateAsync(id,entity);
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        Guid updatedBy = await _userService.GetLoggedInUserAsync();
        return await _repository.DeleteAsync(id, updatedBy);
    }
    public async Task<bool> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active)
    {
        Guid updatedBy = await _userService.GetLoggedInUserAsync();
        return await _repository.ChangeStatusAsync(id, updatedBy, status);
    }
}