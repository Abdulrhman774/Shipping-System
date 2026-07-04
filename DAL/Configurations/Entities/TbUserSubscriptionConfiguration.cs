using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbUserSubscriptionConfiguration : BaseEntityConfiguration<TbUserSubscription>
    {
        public override void Configure(EntityTypeBuilder<TbUserSubscription> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.PackageId).IsRequired();
            builder.Property(e => e.SubscriptionDate).HasColumnType("datetime").IsRequired();

            builder.HasOne(d => d.Package)
                   .WithMany(p => p.TbUserSubscriptions)
                   .HasForeignKey(d => d.PackageId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
