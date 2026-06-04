using Domain.Entities;
using BL.DTOs.PaymentMethod;
using BL.Contracts.IServices;
using DAL.Contracts;
using BL.Mapping;

namespace BL.Services;

public class PaymentMethodService 
    : BaseService<TbPaymentMethod, PaymentMethodDto, CreatePaymentMethodDto, UpdatePaymentMethodDto>, IPaymentMethodService
{
    public PaymentMethodService(IGenericRepository<TbPaymentMethod> repository, IMapper mapper) 
        : base(repository, mapper) { }
}
