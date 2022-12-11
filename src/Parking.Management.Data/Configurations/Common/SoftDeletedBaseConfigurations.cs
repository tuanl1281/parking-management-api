using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parking.Management.Data.Entities.Common;

namespace Parking.Management.Data.Configurations.Common;

public abstract class SoftDeletedConfiguration<T>: IEntityTypeConfiguration<T> where T : SoftDeletedBaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(_ => _.Id)
            .IsUnique();

        ConfigureMoreProperties(builder);
    }

    protected abstract void ConfigureMoreProperties(EntityTypeBuilder<T> builder);
}

