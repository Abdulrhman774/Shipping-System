using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbShippmentConfiguration : IEntityTypeConfiguration<TbShippment>
    {
        public void Configure(EntityTypeBuilder<TbShippment> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.PackageValue).HasColumnType("decimal(8, 4)");
            builder.Property(e => e.ShippingDate).HasColumnType("datetime");
            builder.Property(e => e.ShippingRate).HasColumnType("decimal(8, 4)");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            builder.HasOne(d => d.PaymentMethod).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_TbShippments_TbPaymentMethods");

            builder.HasOne(d => d.Receiver).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippments_TbUserReceivers");

            builder.HasOne(d => d.Sender).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippments_TbUserSebders");

            builder.HasOne(d => d.ShippingType).WithMany(p => p.TbShippments)
                .HasForeignKey(d => d.ShippingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbShippments_TbShippingTypes");
        }
    }
}
