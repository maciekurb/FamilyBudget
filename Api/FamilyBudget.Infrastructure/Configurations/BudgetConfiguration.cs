using FamilyBudget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudget.Infrastructure.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(b => b.Owner)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.OwnerId);

        builder.HasMany(b => b.Incomes)
            .WithOne(i => i.Budget)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Expenses)
            .WithOne(e => e.Budget)
            .OnDelete(DeleteBehavior.Cascade);
    }
}