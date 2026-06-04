using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbUserSenderConfiguration : IEntityTypeConfiguration<TbUserSender>
    {
        public void Configure(EntityTypeBuilder<TbUserSender> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.Email).HasMaxLength(200);
            builder.Property(e => e.Phone).HasMaxLength(200);
            builder.Property(e => e.SenderName).HasMaxLength(200);
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            builder.HasOne(d => d.City).WithMany(p => p.TbUserSebders)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbUserSebders_TbCities");
        }
    }
}
