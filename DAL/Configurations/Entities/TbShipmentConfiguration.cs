using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities;

public class TbShipmentConfiguration : BaseEntityConfiguration<TbShipment>
{
    public override void Configure(EntityTypeBuilder<TbShipment> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.ShippingDate).HasColumnType("datetime").IsRequired();
        builder.Property(e => e.SenderId).IsRequired();
        builder.Property(e => e.ReceiverId).IsRequired();
        builder.Property(e => e.ShippingTypeId).IsRequired();
        builder.Property(e => e.Width).HasColumnType("float").IsRequired();
        builder.Property(e => e.Height).HasColumnType("float").IsRequired();
        builder.Property(e => e.Weight).HasColumnType("float").IsRequired();
        builder.Property(e => e.Length).HasColumnType("float").IsRequired();
        builder.Property(e => e.PackageValue).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(e => e.ShippingRate).HasColumnType("decimal(8, 4)").IsRequired();
        builder.Property(e => e.PaymentMethodId);
        builder.Property(e => e.UserSubscriptionId);
        builder.Property(e => e.TrackingNumber).HasMaxLength(100);
        builder.Property(e => e.ReferenceId);

        builder.HasOne(d => d.PaymentMethod).WithMany(p => p.TbShippments)
               .HasForeignKey(d => d.PaymentMethodId);

        builder.HasOne(d => d.Receiver).WithMany(p => p.TbShippments)
               .HasForeignKey(d => d.ReceiverId)
               .OnDelete(DeleteBehavior.ClientSetNull);


        builder.HasOne(d => d.Sender).WithMany(p => p.TbShippments)
               .HasForeignKey(d => d.SenderId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.ShippingType).WithMany(p => p.TbShipments)
               .HasForeignKey(d => d.ShippingTypeId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.ShippingPackaging)
               .WithMany(p => p.TbShipments)
               .HasForeignKey(d => d.ShippingPackagingId);
    }
}
