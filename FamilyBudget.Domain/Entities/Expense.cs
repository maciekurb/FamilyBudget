using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;

namespace FamilyBudget.Domain.Entities;

public class Expense : BaseEntity
{
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public Category Category { get; internal set; }

    internal Expense() { }

    public static Result<Expense> Create(decimal amount, string description, Category? category) =>
        Result.Success()
            .Ensure(() => amount > 0, "Amount must be greater than zero.")
            .Ensure(() => string.IsNullOrEmpty(description) == false, "Description cannot be empty.")
            .Ensure(() => category != null, "Expense must be linked to some category.")
            .Map(() => new Expense { Amount = amount, Description = description, Category = category! });
}
