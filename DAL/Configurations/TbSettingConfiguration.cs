using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbSettingConfiguration : IEntityTypeConfiguration<TbSetting>
    {
        public void Configure(EntityTypeBuilder<TbSetting> builder)
        {
            builder.ToTable("TbSetting");

            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        }
    }
}
