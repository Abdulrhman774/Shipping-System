using BL.DTOs.Base;

namespace BL.DTOs.ShippingType
{
    public class ShippingTypeDto : BaseDto
    {
        public string? ShippingTypeAname { get; set; }
        public string? ShippingTypeEname { get; set; }
        public double ShippingFactor { get; set; }
    }
}
