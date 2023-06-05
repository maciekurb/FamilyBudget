using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Category
{
    public string Name { get; internal set; }

    internal Category()
    {
        
    }
    
    public static Category Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be empty.");

        return new Category { Name = name };
    }
}