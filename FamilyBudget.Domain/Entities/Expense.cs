using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Expense : BaseEntity
{
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public Category Category { get; internal set; }

    internal Expense() { }

    public static Expense Create(decimal amount, string description, Category category)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        if (string.IsNullOrEmpty(description))
            throw new DomainException("Description cannot be empty.");

        if (category == null)
            throw new DomainException("Category cannot be null.");

        return new Expense { Amount = amount, Description = description, Category = category };
    }
}
