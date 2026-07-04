using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbUserReceiverConfiguration : BaseEntityConfiguration<TbUserReceiver>
    {
        public override void Configure(EntityTypeBuilder<TbUserReceiver> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ReceiverName).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Phone).HasMaxLength(200).IsRequired();
            builder.Property(e => e.PostalCode).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Contact).HasMaxLength(200).IsRequired();
            builder.Property(e => e.OtherAddress).HasMaxLength(500);
            builder.Property(e => e.IsDefaultAddress).IsRequired();
            builder.Property(e => e.Address).HasMaxLength(500).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.CityId).IsRequired();

            builder.HasOne(d => d.City)
                   .WithMany(p => p.TbUserReceivers)
                   .HasForeignKey(d => d.CityId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
