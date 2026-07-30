using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities;

public class TbUserSenderConfiguration
    : SharedSenderReceiverConfiguration<TbUserSender>
{
    public override void Configure(EntityTypeBuilder<TbUserSender> builder)
    {
        base.Configure(builder);

        builder.HasOne(e => e.City)
            .WithMany(c => c.TbUserSenders)
            .HasForeignKey(e => e.CityId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}