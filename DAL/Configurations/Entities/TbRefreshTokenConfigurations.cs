using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations.Entities
{
    public class TbRefreshTokenConfigurations
    : BaseEntityConfiguration<TbRefreshToken>
    {
        public override void Configure(EntityTypeBuilder<TbRefreshToken> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Token)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.Expires)
                   .HasColumnType("datetime");

            builder.Property(x => x.RevokedDate)
                   .HasColumnType("datetime");

            builder.HasIndex(x => x.Token)
                   .IsUnique();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.RefreshTokens)
                   .HasForeignKey(x => x.UserId);
        }
    }
}
