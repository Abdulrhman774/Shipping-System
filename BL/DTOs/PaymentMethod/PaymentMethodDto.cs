using BL.DTOs.Base;

namespace BL.DTOs.PaymentMethod
{
    public class PaymentMethodDto : BaseDto
    {
        public string? MethdAname { get; set; }
        public string? MethodEname { get; set; }
        public double? Commission { get; set; }
    }
}
