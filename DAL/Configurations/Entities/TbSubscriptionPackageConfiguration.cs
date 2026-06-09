using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbSubscriptionPackageConfiguration : BaseEntityConfiguration<TbSubscriptionPackage>
    {
        public override void Configure(EntityTypeBuilder<TbSubscriptionPackage> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PackageName).HasMaxLength(200).IsRequired();
            builder.Property(e => e.ShippimentCount).HasColumnType("int").IsRequired();
            builder.Property(e => e.NumberOfKiloMeters).HasColumnType("float").IsRequired();
            builder.Property(e => e.TotalWeight).HasColumnType("float").IsRequired();

        }
    }
}
