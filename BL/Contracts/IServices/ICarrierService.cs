using Domain.Entities;
using BL.DTOs.Carrier;

namespace BL.Contracts.IServices;

public interface ICarrierService 
    : IBaseService<TbCarrier, CarrierDto, CreateCarrierDto, UpdateCarrierDto> { }
