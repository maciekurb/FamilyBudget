using System.Linq.Expressions;
using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure.Configurations;
using FamilyBudget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FamilyBudget.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CreatedAtSaveChangesInterceptor());
        optionsBuilder.AddInterceptors(new ModifiedAtSaveChangesInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetConfiguration).Assembly);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Set global query filter for all entities implementing ISoftDelete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression<ISoftDelete>(e => !e.IsDeleted, entityType.ClrType));
            }
        }
    }

    private static LambdaExpression ConvertFilterExpression<TInterface>(
        Expression<Func<TInterface, bool>> filterExpression,
        Type entityType)
    {
        var newParam = Expression.Parameter(entityType);
        var newBody =
            ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

        return Expression.Lambda(newBody, newParam);
    }
}