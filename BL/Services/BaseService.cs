using BL.Common.Results;
using BL.Contract;
using BL.Contract.IServices;
using BL.Mapping;
using DAL.Contracts;
using Domain.Shared;
using Domain.Entities;

namespace BL.Services;
public class BaseService<T, TDto, TCreateDto, TUpdateDto> : IBaseService<T, TDto, TCreateDto, TUpdateDto> where T : BaseEntity
{
    protected readonly IGenericRepository<T> _repository;
    protected readonly IBaseMapper _mapper;
    protected readonly IUserService _userService;
    protected readonly IUnitOfWork _unitOfWork;
    protected virtual bool AutoSave { get; set; } = false;
    public BaseService(IUnitOfWork unitOfWork, IBaseMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.Repository<T>();
    }
    public async Task<Result<IEnumerable<TDto>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return _mapper.MapList<T, TDto>(list);
    }

    public async Task<Result<TDto>> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
            return Error.NotFound(
                $"{typeof(T).Name}.NotFound",
                $"{typeof(T).Name} was not found.");

        return _mapper.Map<T, TDto>(entity);
    }

    public virtual async Task<Result> UpdateAsync(Guid id, TUpdateDto dto, bool? autoSave = null)
    {
        var entity = _mapper.Map<TUpdateDto, T>(dto);

        entity.Id = id;
        entity.UpdatedBy = await _userService.GetLoggedInUserAsync();

        var shouldSave = autoSave ?? AutoSave;
        var updated = await _repository.UpdateAsync(id, entity, shouldSave);

        if (!updated)
            return Error.NotFound(
                $"{typeof(T).Name}.NotFound",
                $"{typeof(T).Name} was not found.");

        return Result.Success();
    }

    public virtual async Task<Result> DeleteAsync(Guid id, bool? autoSave = null)
    {
        var shouldSave = autoSave ?? AutoSave;
        var deleted = await _repository.DeleteAsync(
            id,
            await _userService.GetLoggedInUserAsync());

        if (!deleted)
            return Error.NotFound(
                $"{typeof(T).Name}.NotFound",
                $"{typeof(T).Name} was not found.");

        return Result.Success();
    }

    public virtual async Task<Result> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active, bool? autoSave = null)
    {
        var shouldSave = autoSave ?? AutoSave;
        var changed = await _repository.ChangeStatusAsync(
            id,
            await _userService.GetLoggedInUserAsync(),
            status);

        if (!changed)
            return Error.NotFound(
                $"{typeof(T).Name}.NotFound",
                $"{typeof(T).Name} was not found.");

        return Result.Success();
    }

    public virtual async Task<Result<T>> AddAsync(TCreateDto dto, bool? autoSave = null)
    {
        var entity = _mapper.Map<TCreateDto, T>(dto);

        entity.CreatedBy = await _userService.GetLoggedInUserAsync();
        entity.CreatedDate = DateTime.UtcNow;
        entity.CurrentState = enEntityState.Active;

        var shouldSave = autoSave ?? AutoSave;
        await _repository.CreateAsync(entity, shouldSave);

        return Result<T>.Success(entity);
    }
}
