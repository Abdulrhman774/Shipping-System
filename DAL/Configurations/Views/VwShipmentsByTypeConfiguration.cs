// DAL/Configurations/Views/VwShipmentsByTypeConfiguration.cs
using Domain.Entities.Views;
using Domain.Entities.Views.Shipment.Statistics_Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Views
{
    public class VwShipmentsByTypeConfiguration : IEntityTypeConfiguration<VwShipmentsByType>
    {
        public void Configure(EntityTypeBuilder<VwShipmentsByType> builder)
        {
            builder.HasNoKey();
            builder.ToView("vw_ShipmentsByType");
        }
    }
}