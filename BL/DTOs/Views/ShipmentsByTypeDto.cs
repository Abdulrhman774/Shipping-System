namespace BL.DTOs.Views
{
    public class ShipmentsByTypeDto
    {
        public Guid ShippingTypeId { get; set; }
        public string? ShippingTypeName { get; set; }

        public enShipmentStatus Status { get; set; }
        public string? StatusName { get; set; }

        public int ShipmentCount { get; set; }
        public decimal? TotalPackageValue { get; set; }
        public decimal? TotalShippingRate { get; set; }
        public decimal? AverageShippingRate { get; set; }
    }
}