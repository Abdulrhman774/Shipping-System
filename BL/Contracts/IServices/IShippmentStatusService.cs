using Domain.Entities;
using BL.DTOs.ShippmentStatus;

namespace BL.Contracts.IServices;

public interface IShippmentStatusService 
    : IBaseService<TbShippmentStatus, ShippmentStatusDto, CreateShippmentStatusDto, UpdateShippmentStatusDto> { }
