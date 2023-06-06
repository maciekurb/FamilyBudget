using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Exceptions;

namespace FamilyBudget.Domain.Entities;

public class Budget : BaseEntity
{
    public string Name { get; internal set; }
    public User? Owner { get; internal set; }
    private readonly List<User> _sharedUsers = new List<User>();
    public IReadOnlyCollection<User> SharedUsers => _sharedUsers.AsReadOnly();
    private readonly List<Income> _incomes = new List<Income>();
    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    private readonly List<Expense> _expenses = new List<Expense>();
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    internal Budget()
    {
        
    }

    public static Result<Budget> Create(string name, User? owner) =>
        Result.Success()
            .Ensure(() => string.IsNullOrEmpty(name) == false, "Name cannot be empty.")
            .Ensure(() => owner != null, "Owner cannot be null.")
            .Map(() => new Budget
            {
                Name = name,
                Owner = owner
            });
    
    public Result Update(string name) =>
        Result.Success()
            .Ensure(() => string.IsNullOrEmpty(name) == false, "Name cannot be empty.")
            .Tap(() => Name = name);
    
    public Result ShareBudget(IEnumerable<User> users)
        => Result.Success()
            .Map(() => users.Select(ShareBudget))
            .Map(results => Result.Combine(results))
            .Ensure(combinedResult => combinedResult.IsSuccess, combinedResult => combinedResult.Error)
            .Tap(() => _sharedUsers.AddRange(users));

    public Result ShareBudget(User? user)
        => Result.Success()
            .Ensure(() => user != null, "User not found")
            .Ensure(() => _sharedUsers.Any(u => u.Id == user.Id) == false, "User is already shared with this budget.")
            .Tap(() => _sharedUsers.Add(user!));
    
    public Result AddIncomes(IEnumerable<Income> incomes)
        => Result.Success()
            .Map(() => incomes.Select(AddIncome))
            .Map(results => Result.Combine(results))
            .Ensure(combinedResult => combinedResult.IsSuccess, combinedResult => combinedResult.Error)            
            .Tap(() => _incomes.AddRange(incomes));
    
    public Result AddIncome(Income? income)
        => Result.Success()
            .Ensure(() => income != null, "Income cannot be null")
            .Tap(() => _incomes.Add(income!));
    
    public Result AddExpenses(IEnumerable<Expense> expenses)
        => Result.Success()
            .Map(() => expenses.Select(AddExpense))
            .Map(results => Result.Combine(results))
            .Ensure(combinedResult => combinedResult.IsSuccess, combinedResult => combinedResult.Error)            
            .Tap(() => _expenses.AddRange(expenses));
    
    public Result AddExpense(Expense? expense)
        => Result.Success()
            .Ensure(() => expense != null, "Expense cannot be null")
            .Tap(() => _expenses.Add(expense!));
}