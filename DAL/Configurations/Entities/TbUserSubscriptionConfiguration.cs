using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbUserSubscriptionConfiguration : BaseEntityConfiguration<TbUserSubscription>
    {
        public override void Configure(EntityTypeBuilder<TbUserSubscription> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.PackageId).IsRequired();
            builder.Property(e => e.SubscriptionDate).HasColumnType("datetime").IsRequired();

            builder.Property(e => e.UsedShipmentCount).HasDefaultValue(0).IsRequired();
            builder.Property(e => e.UsedTotalWeight).HasColumnType("float").HasDefaultValue(0).IsRequired();
            builder.Property(e => e.UsedTotalDistance).HasColumnType("float").HasDefaultValue(0).IsRequired();
            builder.Property(e => e.ExpiryDate).HasColumnType("datetime");
            builder.Property(e => e.IsActive).HasDefaultValue(true).IsRequired();
            builder.HasIndex(e => e.UserId);

            builder.HasOne(d => d.Package)
                   .WithMany(p => p.TbUserSubscriptions)
                   .HasForeignKey(d => d.PackageId)
                   .OnDelete(DeleteBehavior.ClientSetNull);


            builder.HasOne(d => d.Package)
                   .WithMany(p => p.TbUserSubscriptions)
                   .HasForeignKey(d => d.PackageId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
