using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities;

public abstract class SharedSenderReceiverConfiguration<T>
    : BaseEntityConfiguration<T>
    where T : SharedSenderReceiver
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.PostalCode)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Contact)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.OtherAddress)
            .HasMaxLength(500);

        builder.Property(e => e.IsDefaultAddress)
            .IsRequired();

        builder.Property(e => e.Address)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(e => e.CityId)
            .IsRequired();

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.Phone).IsUnique();

        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}