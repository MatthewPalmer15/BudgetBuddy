using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceProvider = BudgetBuddy.Data.Entities.ServiceProvider;

namespace BudgetBuddy.Data.Configurations;

internal class ServiceProviderEntityConfiguration : IEntityTypeConfiguration<ServiceProvider>
{
    public void Configure(EntityTypeBuilder<ServiceProvider> builder)
    {
        builder.ToTable("tbl_ServiceProvider");

        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id)
            .IsUnique();

        builder.Property(e => e.Id)
            .HasColumnName("ServiceProviderId")
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

        builder.Property(e => e.CategoryId)
            .IsRequired(false);

        builder.HasOne(e => e.Category)
            .WithOne()
            .HasForeignKey<ServiceProvider>(e => e.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(e => e.Rank)
            .HasDefaultValue(0)
            .IsRequired();
    }
}