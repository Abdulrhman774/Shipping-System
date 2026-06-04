using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbShippmentStatusConfiguration : IEntityTypeConfiguration<TbShippmentStatus>
    {
        public void Configure(EntityTypeBuilder<TbShippmentStatus> builder)
        {
            builder.ToTable("TbShippmentStatus");

            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

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
