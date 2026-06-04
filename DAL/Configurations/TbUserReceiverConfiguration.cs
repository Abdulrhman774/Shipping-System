using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TbUserReceiverConfiguration : IEntityTypeConfiguration<TbUserReceiver>
    {
        public void Configure(EntityTypeBuilder<TbUserReceiver> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.Email).HasMaxLength(200);
            builder.Property(e => e.Phone).HasMaxLength(200);
            builder.Property(e => e.ReceiverName).HasMaxLength(200);
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            builder.HasOne(d => d.City).WithMany(p => p.TbUserReceivers)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbUserReceivers_TbCities");
        }
    }
}
