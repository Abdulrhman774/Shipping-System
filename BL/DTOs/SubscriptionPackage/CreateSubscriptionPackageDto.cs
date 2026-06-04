namespace BL.DTOs.SubscriptionPackage
{
    public class CreateSubscriptionPackageDto
    {
        public string PackageName { get; set; } = null!;
        public int ShippimentCount { get; set; }
        public double NumberOfKiloMeters { get; set; }
        public double TotalWeight { get; set; }
    }
}
