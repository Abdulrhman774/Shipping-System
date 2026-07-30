using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities;

public class TbUserReceiverConfiguration
    : SharedSenderReceiverConfiguration<TbUserReceiver>
{
    public override void Configure(EntityTypeBuilder<TbUserReceiver> builder)
    {
        base.Configure(builder);

        builder.HasOne(e => e.City)
            .WithMany(c => c.TbUserReceivers)
            .HasForeignKey(e => e.CityId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}