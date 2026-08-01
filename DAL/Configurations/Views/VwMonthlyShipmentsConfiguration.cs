// DAL/Configurations/Views/VwMonthlyShipmentsConfiguration.cs
using Domain.Entities.Views;
using Domain.Entities.Views.Shipment.Statistics_Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Views
{
    public class VwMonthlyShipmentsConfiguration : IEntityTypeConfiguration<VwMonthlyShipments>
    {
        public void Configure(EntityTypeBuilder<VwMonthlyShipments> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_MonthlyShipments");
        }
    }
}