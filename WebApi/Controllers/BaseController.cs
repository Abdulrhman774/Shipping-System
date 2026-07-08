using BL.Contract;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<TService, T, TDto, TCreateDto, TUpdateDto> : ControllerBase
    where TService : IBaseService<T, TDto, TCreateDto, TUpdateDto>
    where T : BaseEntity
{
    protected readonly TService _service;

    protected BaseController(TService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult(this);
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return result.ToActionResult(this);
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] TUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result.ToActionResult(this);
    }

    [HttpDelete]
    public virtual async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result.ToActionResult(this);
    }

    [HttpPatch("{id:guid}/status")]
    public virtual async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] enEntityState status = enEntityState.Active)
    {
        var result = await _service.ChangeStatusAsync(id, status);
        return result.ToActionResult(this);
    }
}