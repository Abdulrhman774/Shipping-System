using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AspNetUsers");

        builder.Property(u => u.FullName)
               .IsRequired()
               .HasMaxLength(150);
        

        builder.Property(u => u.Gender)
               .IsRequired()
               .HasConversion<byte>();

        builder.Property(u => u.ImageUrl)
               .IsRequired()
               .HasMaxLength(800);               
    }
}
