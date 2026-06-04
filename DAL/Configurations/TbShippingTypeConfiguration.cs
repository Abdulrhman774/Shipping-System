using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbShippingTypeConfiguration : IEntityTypeConfiguration<TbShippingType>
    {
        public void Configure(EntityTypeBuilder<TbShippingType> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.ShippingTypeAname)
                .HasMaxLength(200)
                .HasColumnName("ShippingTypeAName");
            builder.Property(e => e.ShippingTypeEname)
                .HasMaxLength(200)
                .HasColumnName("ShippingTypeEName");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");
        }
    }
}
