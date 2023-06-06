using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Infrastructure.Providers;

[Injectable]
public interface ICategoryProvider
{
    Task<Result<Category>> GetOrCreateAsync(string categoryName, CancellationToken cancellationToken);
}

public class CategoryProvider : ICategoryProvider
{
    private readonly AppDbContext _context;

    public CategoryProvider(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Category>> GetOrCreateAsync(string categoryName, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName, cancellationToken);

        if (category != null)
            return category;

        var newCategory = Category.Create(categoryName);
        if (newCategory.IsFailure)
            return Result.Failure<Category>(newCategory.Error);

        _context.Categories.Add(newCategory.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return newCategory;
    }
}