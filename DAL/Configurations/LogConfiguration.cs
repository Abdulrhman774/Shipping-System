using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Message).HasColumnType("nvarchar(max)");
            builder.Property(e => e.MessageTemplate).HasColumnType("nvarchar(max)");
            builder.Property(e => e.Level).HasMaxLength(16);
            builder.Property(e => e.TimeStamp).HasColumnType("datetime");
            builder.Property(e => e.Exception).HasColumnType("nvarchar(max)");
            builder.Property(e => e.Properties).HasColumnType("nvarchar(max)");
        }
    }
}
