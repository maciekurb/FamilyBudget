using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;

namespace FamilyBudget.Domain.Entities;

public class Budget : BaseEntity
{
    public string Name { get; internal set; }
    public User? Owner { get; internal set; }
    public Guid OwnerId { get; set; }
    public virtual List<User> SharedUsers { get; internal set; } = new List<User>();
    public virtual List<Income> Incomes { get; internal set; } = new List<Income>();
    public virtual List<Expense> Expenses { get; internal set; } = new List<Expense>();

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
            .Tap(() => SharedUsers.AddRange(users));

    public Result ShareBudget(User? user)
        => Result.Success()
            .Ensure(() => user != null, "User not found")
            .Ensure(() => SharedUsers.Any(u => u.Id == user.Id) == false, "User is already shared with this budget.")
            .Tap(() => SharedUsers.Add(user!));
    
    public Result AddIncomes(IEnumerable<Income> incomes)
        => Result.Success()
            .Map(() => incomes.Select(AddIncome))
            .Map(results => Result.Combine(results))
            .Ensure(combinedResult => combinedResult.IsSuccess, combinedResult => combinedResult.Error)            
            .Tap(() => Incomes.AddRange(incomes));
    
    public Result AddIncome(Income? income)
        => Result.Success()
            .Ensure(() => income != null, "Income cannot be null")
            .Tap(() => Incomes.Add(income!));
    
    public Result AddExpenses(IEnumerable<Expense> expenses)
        => Result.Success()
            .Map(() => expenses.Select(AddExpense))
            .Map(results => Result.Combine(results))
            .Ensure(combinedResult => combinedResult.IsSuccess, combinedResult => combinedResult.Error)            
            .Tap(() => Expenses.AddRange(expenses));
    
    public Result AddExpense(Expense? expense)
        => Result.Success()
            .Ensure(() => expense != null, "Expense cannot be null")
            .Tap(() => Expenses.Add(expense!));
}