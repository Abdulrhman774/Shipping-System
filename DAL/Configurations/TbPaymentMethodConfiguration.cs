using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbPaymentMethodConfiguration : IEntityTypeConfiguration<TbPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<TbPaymentMethod> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.MethdAname)
                .HasMaxLength(200)
                .HasColumnName("MethdAName");
            builder.Property(e => e.MethodEname)
                .HasMaxLength(200)
                .HasColumnName("MethodEName");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");
        }
    }
}
