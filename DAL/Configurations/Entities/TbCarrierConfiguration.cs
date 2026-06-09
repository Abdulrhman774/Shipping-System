using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Entities
{
    public class TbCarrierConfiguration : BaseEntityConfiguration<TbCarrier>
    {
        public override void Configure(EntityTypeBuilder<TbCarrier> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CarrierName).HasMaxLength(200);
        }
    }
}
