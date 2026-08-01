// BL/DTOs/Views/ShipmentStatsDto.cs
namespace BL.DTOs.Views
{
    public class ShipmentStatsDto
    {
        public enEntityState CurrentState { get; set; }
        public string? StatusName { get; set; }
        public int ShipmentCount { get; set; }
        public decimal? TotalPackageValue { get; set; }
        public decimal? TotalShippingRate { get; set; }
        public decimal? AverageShippingRate { get; set; }
        public DateTime? OldestShipment { get; set; }
        public DateTime? LatestShipment { get; set; }
    }
}