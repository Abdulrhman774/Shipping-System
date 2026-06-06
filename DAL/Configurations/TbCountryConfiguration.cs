using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbCountryConfiguration : BaseEntityConfiguration<TbCountry>
    {
        public override void Configure(EntityTypeBuilder<TbCountry> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CountryAname)
                .HasMaxLength(200)
                .HasColumnName("CountryAName");
            builder.Property(e => e.CountryEname)
                .HasMaxLength(200)
                .HasColumnName("CountryEName");
        }
    }
}
