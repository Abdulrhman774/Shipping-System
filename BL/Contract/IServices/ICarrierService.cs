using Domain.Entities;
using BL.DTOs.Carrier;

namespace BL.Contract.IServices;

public interface ICarrierService 
    : IBaseService<TbCarrier, CarrierDto, CreateCarrierDto, UpdateCarrierDto> { }
