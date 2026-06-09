using Domain.Entities;
using BL.DTOs.Shippment;

namespace BL.Contract.IServices;

public interface IShippmentService 
    : IBaseService<TbShippment, ShippmentDto, CreateShippmentDto, UpdateShippmentDto> { }
