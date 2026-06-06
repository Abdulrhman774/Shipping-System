using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbShippingTypeConfiguration : BaseEntityConfiguration<TbShippingType>
    {
        public override void Configure(EntityTypeBuilder<TbShippingType> builder)
        {
            base.Configure(builder);


            builder.Property(e => e.ShippingTypeAname)
                .HasMaxLength(200)
                .HasColumnName("ShippingTypeAName");

            builder.Property(e => e.ShippingTypeEname)
                .HasMaxLength(200)
                .HasColumnName("ShippingTypeEName");

            builder.Property(e => e.ShippingFactor).HasColumnType("float").IsRequired();

        }
    }
}
