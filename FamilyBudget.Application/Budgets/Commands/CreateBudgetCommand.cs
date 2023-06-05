using CSharpFunctionalExtensions;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Application.Incomes.DTOs;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure;
using MediatR;

namespace FamilyBudget.Application.Budgets.Commands;

public record CreateBudgetCommand(BudgetDto Dto) : IRequest<Result<Budget>>;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Result<Budget>>
{
    private readonly AppDbContext _appDbContext;
    
    public CreateBudgetCommandHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<Result<Budget>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken) =>
        Result.Success()
            .Map(async () => await _appDbContext.Users.FindAsync(request.Dto.UserId))
            .Ensure(user => user != null, "User not found")
            .Bind(user => Budget.Create(request.Dto.Name, user))
            .CheckIf(_ => request.Dto.Incomes.Any(),
                budget =>
                {
                    var incomes = CreateIncomes(request.Dto.Incomes);
                    if (incomes.IsFailure)
                        return Result.Failure(incomes.Error);

                    return budget.AddIncomes(incomes.Value);
                })
            .CheckIf(_ => request.Dto.Expenses.Any(),
                budget =>
                {
                    var expenses = CreateExpenses(request.Dto.Expenses);
                    if (expenses.IsFailure)
                        return Result.Failure(expenses.Error);

                    return budget.AddExpenses(expenses.Value);
                });

    private Result<List<Income>> CreateIncomes(IEnumerable<IncomeDto> incomeDtos)
    {
        var incomes = new List<Income>();
        foreach (var incomeDto in incomeDtos)
        {
            var categoryResult = Category.Create(incomeDto.Category);
            if (categoryResult.IsFailure)
                return Result.Failure<List<Income>>(categoryResult.Error);

            var incomeResult = Income.Create(incomeDto.Amount, incomeDto.Description, categoryResult.Value);
            if (incomeResult.IsFailure)
                return Result.Failure<List<Income>>(incomeResult.Error);

            incomes.Add(incomeResult.Value);
        }

        return Result.Success(incomes);
    }

    private Result<List<Expense>> CreateExpenses(IEnumerable<ExpenseDto> expensesDtos)
    {
        var incomes = new List<Expense>();
        foreach (var expenseDto in expensesDtos)
        {
            var categoryResult = Category.Create(expenseDto.Category);
            if (categoryResult.IsFailure)
                return Result.Failure<List<Expense>>(categoryResult.Error);

            var expenseResult = Expense.Create(expenseDto.Amount, expenseDto.Description, categoryResult.Value);
            if (expenseResult.IsFailure)
                return Result.Failure<List<Expense>>(expenseResult.Error);

            incomes.Add(expenseResult.Value);
        }

        return Result.Success(incomes);
    }
}