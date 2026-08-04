namespace BL.DTOs.Views
{
    public class MonthlyShipmentsDto
    {
        public int ShipmentYear { get; set; }
        public int ShipmentMonth { get; set; }
        public string? MonthName { get; set; }

        public enShipmentStatus Status { get; set; }
        public string? StatusName { get; set; }

        public int TotalShipments { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? AverageRate { get; set; }
    }
}