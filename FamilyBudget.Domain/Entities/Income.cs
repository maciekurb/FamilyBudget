using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Income
{
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public Category Category { get; internal set; }
    
    internal Income() { }

    public static Income Create(decimal amount, string description, Category category)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero.");

        if (string.IsNullOrEmpty(description))
            throw new DomainException("Description cannot be empty.");

        if (category == null)
            throw new DomainException("Category cannot be null.");

        return new Income { Amount = amount, Description = description, Category = category };
    }
}