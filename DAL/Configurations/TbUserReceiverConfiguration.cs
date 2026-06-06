using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbUserReceiverConfiguration : BaseEntityConfiguration<TbUserReceiver>
    {
        public override void Configure(EntityTypeBuilder<TbUserReceiver> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ReceiverName).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Phone).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Address).HasMaxLength(500).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.CityId).IsRequired();

            builder.HasOne(d => d.City)
                   .WithMany(p => p.TbUserReceivers)
                   .HasForeignKey(d => d.CityId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_TbUserReceivers_TbCities");
        }
    }
}
