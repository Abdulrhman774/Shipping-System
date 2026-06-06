using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbSettingConfiguration : BaseEntityConfiguration<TbSetting>
    {
        public override void Configure(EntityTypeBuilder<TbSetting> builder)
        {
            base.Configure(builder);

            builder.ToTable("TbSetting");

            builder.Property(e => e.KiloMeterRate).HasColumnType("float");
            builder.Property(e => e.KilooGramRate).HasColumnType("float");
        }
    }
}
