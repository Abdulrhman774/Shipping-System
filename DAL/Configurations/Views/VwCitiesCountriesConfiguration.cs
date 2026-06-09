using Domain.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Views;

public class VwCitiesCountriesConfiguration : IEntityTypeConfiguration<VwCitiesCountries>
{
    public void Configure(EntityTypeBuilder<VwCitiesCountries> builder)
    {
        builder.HasNoKey();
        builder.ToView("vw_CitiesCountries");
    }
}