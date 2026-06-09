using DAL.Context;
using DAL.Contracts.IRepositories;
using DAL.Repositories.Generic;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories;

public class PaymentMethodRepository : GenericRepository<TbPaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ShippingDbContext context, ILogger<PaymentMethodRepository> logger) : base(context, logger)
    {
    }
}