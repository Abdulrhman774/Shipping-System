using BL.Contract.IServices;
using BL.DTOs.PaymentMethod;
using BL.DTOs.ShippingType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodService _service;
    private readonly ILogger<PaymentMethodController> _logger;

    public PaymentMethodController(IPaymentMethodService service, ILogger<PaymentMethodController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _service.GetByIdAsync(id);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentMethodDto dto)
    {
        var result = await _service.AddAsync(dto);
        return result ? Ok("Created Successfully") : BadRequest("Creation Failed");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePaymentMethodDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result ? Ok("Updated Successfully") : BadRequest("Update Failed");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok("Deleted Successfully") : BadRequest("Delete Failed");
    }

    [HttpPatch("{id:guid}/status/{status:int}")]
    public async Task<IActionResult> ChangeStatus(Guid id, enEntityState status)
    {
        var result = await _service.ChangeStatusAsync(id, status);
        return result ? Ok("Status Changed Successfully") : BadRequest("Status Change Failed");
    }
}