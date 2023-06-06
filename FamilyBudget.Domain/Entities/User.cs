using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; internal set; }
    public string Email { get; internal set; }
    
    private readonly List<Budget> _budgets = new List<Budget>();
    public IReadOnlyCollection<Budget> Budgets => _budgets.AsReadOnly();

    internal User()
    {
    }

    public static User Create(string username, string email)
    {
        if(string.IsNullOrEmpty(username))
            throw new DomainException("Username cannot be empty.");
        if(string.IsNullOrEmpty(email))
            throw new DomainException("Email cannot be empty.");
        
        return new User
        {
            Username = username,
            Email = email
        };
    }
    
    public void AddBudget(Budget budget)
    {
        if(budget == null)
            throw new DomainException("Budget cannot be null.");

        _budgets.Add(budget);
    }
}