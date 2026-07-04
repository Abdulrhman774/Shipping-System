using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbShippingPackagingConfiguration : BaseEntityConfiguration<TbShippingPackaging>
    {
        public override void Configure(EntityTypeBuilder<TbShippingPackaging> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ShippingPackagingAname)
                .HasMaxLength(100);



            builder.Property(e => e.ShippingPackagingEname)
                .HasMaxLength(100);



        }
    }
}
