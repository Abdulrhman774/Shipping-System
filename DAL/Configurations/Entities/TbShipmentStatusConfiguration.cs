using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbShipmentStatusConfiguration : BaseEntityConfiguration<TbShipmentStatus>
    {
        public override void Configure(EntityTypeBuilder<TbShipmentStatus> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CarrierId).IsRequired();
            builder.Property(e => e.ShipmentId);
            builder.Property(e => e.Notes).HasColumnType("nvarchar(max)");


            builder.HasOne(d => d.Carrier).WithMany(p => p.TbShippmentStatuses)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.HasOne(d => d.Shippment).WithMany(p => p.TbShipmentStatuses)
                .HasForeignKey(d => d.ShipmentId);

        }
    }
}
