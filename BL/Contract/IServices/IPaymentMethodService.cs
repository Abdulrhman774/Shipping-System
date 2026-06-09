using Domain.Entities;
using BL.DTOs.PaymentMethod;

namespace BL.Contract.IServices;

public interface IPaymentMethodService 
    : IBaseService<TbPaymentMethod, PaymentMethodDto, CreatePaymentMethodDto, UpdatePaymentMethodDto> { }
