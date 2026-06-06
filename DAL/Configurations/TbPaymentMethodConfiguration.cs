using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbPaymentMethodConfiguration : BaseEntityConfiguration<TbPaymentMethod>
    {
        public override void Configure(EntityTypeBuilder<TbPaymentMethod> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MethdAname)
                .HasMaxLength(200)
                .HasColumnName("MethdAName");

            builder.Property(e => e.MethodEname)
                .HasMaxLength(200)
                .HasColumnName("MethodEName");

            builder.Property(e => e.Commission).HasColumnType("float");
        }
    }
}
