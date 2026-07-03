using BL.Contract.IServices;
using BL.DTOs.PaymentMethod;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodService _service;


public PaymentMethodController(IPaymentMethodService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return result.ToActionResult(this);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodDto dto)
    {
        var result = await _service.AddAsync(dto);

        return result.ToActionResult(this);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePaymentMethodDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        return result.ToActionResult(this);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        return result.ToActionResult(this);
    }


    [HttpPatch("{id:guid}/status/{status:int}")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        enEntityState status)
    {
        var result = await _service.ChangeStatusAsync(id, status);

        return result.ToActionResult(this);
    }


}
