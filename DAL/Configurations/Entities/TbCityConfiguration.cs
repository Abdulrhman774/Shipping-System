using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbCityConfiguration : BaseEntityConfiguration<TbCity>
    {
        public override void Configure(EntityTypeBuilder<TbCity> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CityAname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CityAName");


            builder.Property(e => e.CityEname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CityEName");

            builder.Property(e => e.CountryId).IsRequired();

            builder.HasOne(d => d.Country).WithMany(p => p.TbCities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbCities_TbCountries");
        }
    }
}
