using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Configurations.Entities;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("(newid())");

        builder.Property(e => e.CreatedDate)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.UpdatedDate)
               .HasColumnType("datetime");

        builder.Property(e => e.CurrentState)
               .HasConversion<byte>();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.UpdatedBy);


        // Global query filter to exclude soft-deleted entities
        builder.HasQueryFilter(x => x.CurrentState != enEntityState.Deleted);
    }


}