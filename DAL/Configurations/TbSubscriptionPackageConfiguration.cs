using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbSubscriptionPackageConfiguration : IEntityTypeConfiguration<TbSubscriptionPackage>
    {
        public void Configure(EntityTypeBuilder<TbSubscriptionPackage> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.PackageName).HasMaxLength(200);
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");
        }
    }
}
