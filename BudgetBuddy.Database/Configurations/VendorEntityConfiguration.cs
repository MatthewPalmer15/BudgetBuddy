using BudgetBuddy.Database.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetBuddy.Database.Configurations;

internal class VendorEntityConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("tbl_Vendor");

        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id)
            .IsUnique();

        builder.Property(e => e.Id)
            .HasColumnName("VendorId")
            .HasColumnOrder(1)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(e => e.Deleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(e => !e.Deleted);

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .HasDefaultValue(string.Empty)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.Rank)
            .HasDefaultValue(0)
            .IsRequired();
    }
}