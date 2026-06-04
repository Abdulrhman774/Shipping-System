using Domain.Entities;
using BL.DTOs.Shippment;

namespace BL.Contracts.IServices;

public interface IShippmentService 
    : IBaseService<TbShippment, ShippmentDto, CreateShippmentDto, UpdateShippmentDto> { }
