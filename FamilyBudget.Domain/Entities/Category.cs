using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; internal set; }

    internal Category()
    {
        
    }

    public static Result<Category> Create(string name) =>
        Result.Success()
            .Ensure(() => string.IsNullOrEmpty(name) == false, "Name cannot be empty.")
            .Map(() => new Category
            {
                Name = name
            });
}