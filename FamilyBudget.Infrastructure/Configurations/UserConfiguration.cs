using FamilyBudget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudget.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder
            .HasMany(u => u.Budgets)
            .WithOne(b => b.Owner)
            .HasForeignKey(b => b.OwnerId);

        builder
            .HasMany(u => u.SharedBudgets)
            .WithMany(b => b.SharedUsers)
            .UsingEntity(j => j.ToTable("UserBudgets"));
    }
}