using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbShippmentStatusConfiguration : BaseEntityConfiguration<TbShippmentStatus>
    {
        public override void Configure(EntityTypeBuilder<TbShippmentStatus> builder)
        {
            base.Configure(builder);
            builder.ToTable("TbShippmentStatus");

            builder.Property(e => e.CarrierId).IsRequired();
            builder.Property(e => e.ShippmentId);
            builder.Property(e => e.Notes).HasColumnType("nvarchar(max)");


            builder.HasOne(d => d.Carrier).WithMany(p => p.TbShippmentStatuses)
                .HasForeignKey(d => d.CarrierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippmentStatus_TbCarriers");

            builder.HasOne(d => d.Shippment).WithMany(p => p.TbShippmentStatuses)
                .HasForeignKey(d => d.ShippmentId)
                .HasConstraintName("FK_TbShippmentStatus_TbShippments");
        }
    }
}
