using BlazorHybrid.Database.Entities.Configuration;
using BlazorHybrid.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorHybrid.Database.Configurations.Configuration;

internal class SettingEntityConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("tbl_Setting");

        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id)
            .IsUnique();

        builder.Property(e => e.Id)
            .HasColumnName("SettingId")
            .HasColumnOrder(1)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(e => e.Deleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(e => !e.Deleted);

        builder.Property(e => e.Key)
            .HasMaxLength(128)
            .HasDefaultValue(string.Empty)
            .IsRequired();

        builder.HasIndex(e => e.Key)
            .IsUnique();

        builder.Property(e => e.Value)
            .HasMaxLength(-1)
            .HasDefaultValue(string.Empty)
            .IsRequired()
            .IsEncrypted();

        builder.Property(e => e.IsSystemManaged)
            .HasDefaultValue(false)
            .IsRequired();
    }
}