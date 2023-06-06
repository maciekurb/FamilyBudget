using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;

namespace FamilyBudget.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; internal set; }
    public virtual ICollection<Income> Incomes { get; private set; }
    public virtual ICollection<Expense> Expenses { get; private set; }

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