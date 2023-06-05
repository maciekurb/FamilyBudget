using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Budget 
{
    public string Name { get; internal set; }
    public User Owner { get; internal set; }
    private readonly List<User> _sharedUsers = new List<User>();
    public IReadOnlyCollection<User> SharedUsers => _sharedUsers.AsReadOnly();
    private readonly List<Income> _incomes = new List<Income>();
    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    private readonly List<Expense> _expenses = new List<Expense>();
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    internal Budget()
    {
        
    }

    public static Budget Create(string name, User owner)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be empty.");

        if (owner == null)
            throw new DomainException("Owner cannot be null.");

        return new Budget { Name = name, Owner = owner };
    }
    
    public void ShareBudget(User user)
    {
        if(user == null)
            throw new DomainException("User cannot be null.");

        if(_sharedUsers.Any(u => u.Id == user.Id))
            throw new DomainException("User is already shared with this budget.");

        _sharedUsers.Add(user);
    }

    public void AddIncome(Income income)
    {
        if(income == null)
            throw new DomainException("Income cannot be null.");

        _incomes.Add(income);
    }

    public void AddExpense(Expense expense)
    {
        if(expense == null)
            throw new DomainException("Expense cannot be null.");

        _expenses.Add(expense);
    }
}