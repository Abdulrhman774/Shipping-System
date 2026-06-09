using Domain.Entities;
using BL.DTOs.ShippmentStatus;

namespace BL.Contract.IServices;

public interface IShippmentStatusService 
    : IBaseService<TbShippmentStatus, ShippmentStatusDto, CreateShippmentStatusDto, UpdateShippmentStatusDto> { }
