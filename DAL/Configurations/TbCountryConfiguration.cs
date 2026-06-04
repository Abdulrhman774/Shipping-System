using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbCountryConfiguration : IEntityTypeConfiguration<TbCountry>
    {
        public void Configure(EntityTypeBuilder<TbCountry> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CountryAname)
                .HasMaxLength(200)
                .HasColumnName("CountryAName");
            builder.Property(e => e.CountryEname)
                .HasMaxLength(200)
                .HasColumnName("CountryEName");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");
        }
    }
}
