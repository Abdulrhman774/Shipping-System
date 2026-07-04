using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;

namespace DAL.Configurations.Entities;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AspNetUsers");

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.SecondName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.ThirdName)
               .HasMaxLength(100);

        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.DateOfBirth)
               .IsRequired()
               .HasColumnType("date");


        builder.Property(u => u.Gender)
               .IsRequired()
               .HasConversion<byte>();

        builder.Property(u => u.ImageUrl)
               .IsRequired()
               .HasMaxLength(800);               
    }
}
