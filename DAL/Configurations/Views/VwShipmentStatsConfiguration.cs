// DAL/Configurations/Views/VwShipmentStatsConfiguration.cs
using Domain.Entities.Views;
using Domain.Entities.Views.Shipment.Statistics_Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Views
{
    public class VwShipmentStatsConfiguration : IEntityTypeConfiguration<vw_ShipmentStats>
    {
        public void Configure(EntityTypeBuilder<vw_ShipmentStats> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_ShipmentStats");
        }
    }
}