using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbShipmentStatusHistoryConfiguration : BaseEntityConfiguration<TbShipmentStatusHistory>
    {
        public override void Configure(EntityTypeBuilder<TbShipmentStatusHistory> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Status)
                   .HasConversion<byte>()
                   .IsRequired();

            builder.Property(e => e.Note).HasMaxLength(500);

            // العلاقة مع TbShipment
            builder.HasOne(h => h.Shipment)
                   .WithMany(s => s.ShipmentStatusHistories)
                   .HasForeignKey(h => h.ShipmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}