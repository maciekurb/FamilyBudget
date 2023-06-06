using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Income : BaseEntity
{
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public Category Category { get; internal set; }
    
    internal Income() { }

    public static Result<Income> Create(decimal amount, string description, Category? category) =>
        Result.Success()
            .Ensure(() => Validate(amount, description, category))
            .Map(() => new Income { Amount = amount, Description = description, Category = category! });

    public Result Update(decimal amount, string description, Category? category) =>
        Result.Success()
            .Ensure(() => Validate(amount, description, category))
            .Tap(() =>
            {
                Amount = amount;
                Description = description;
                Category = category!;
            });
    
    private static Result Validate(decimal amount, string description, Category? category) =>
        Result.Success()
            .Ensure(() => amount > 0, "Amount must be greater than zero.")
            .Ensure(() => string.IsNullOrEmpty(description) == false, "Description cannot be empty.")
            .Ensure(() => category != null, "Expense must be linked to some category.");
}