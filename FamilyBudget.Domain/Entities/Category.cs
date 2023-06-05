using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Category : BaseEntity
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