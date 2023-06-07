using AutoMapper;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Application.Incomes.DTOs;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure;
using FamilyBudget.Infrastructure.Providers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Application.Budgets.Commands;

public record CreateBudgetCommand(Guid UserId, BudgetDto Dto) : IRequest<Result<BudgetDto>>;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Result<BudgetDto>>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly ICategoryProvider _categoryProvider;

    public CreateBudgetCommandHandler(AppDbContext appDbContext, IMapper mapper, ICategoryProvider categoryProvider)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _categoryProvider = categoryProvider;
    }

    public Task<Result<BudgetDto>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken) =>
        Result.Success()
            .Map(async () => await _appDbContext.Users.FindAsync(request.UserId))
            .Ensure(user => user != null, "User not found")
            .Bind(user => Budget.Create(request.Dto.Name, user))
            .CheckIf(_ => request.Dto.Incomes.Any(),
                async budget =>
                {
                    var incomes = await CreateIncomes(request.Dto.Incomes, cancellationToken);
                    if (incomes.IsFailure)
                        return Result.Failure(incomes.Error);

                    return budget.AddIncomes(incomes.Value);
                })
            .CheckIf(_ => request.Dto.Expenses.Any(),
                async budget =>
                {
                    var expenses = await CreateExpenses(request.Dto.Expenses, cancellationToken);
                    if (expenses.IsFailure)
                        return Result.Failure(expenses.Error);

                    return budget.AddExpenses(expenses.Value);
                })
            .CheckIf(_ => request.Dto.SharedToUsersIds.Any(),
                async budget =>
                {
                    var users = await _appDbContext.Users.Where(x => request.Dto.SharedToUsersIds.Contains(x.Id))
                        .ToListAsync(cancellationToken);

                    return budget.ShareBudget(users);
                })
            .Tap(async budget =>
            {
                await _appDbContext.Budgets.AddAsync(budget, cancellationToken);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            })
            .Map(budget => _mapper.Map<BudgetDto>(budget));

    private async Task<Result<List<Income>>> CreateIncomes(IEnumerable<IncomeDto> incomeDtos, CancellationToken cancellationToken)
    {
        var incomes = new List<Income>();
        foreach (var incomeDto in incomeDtos)
        {
            var categoryResult = await _categoryProvider.GetOrCreateAsync(incomeDto.Category, cancellationToken);
            if (categoryResult.IsFailure)
                return Result.Failure<List<Income>>(categoryResult.Error);

            var incomeResult = Income.Create(incomeDto.Amount, incomeDto.Description, categoryResult.Value);
            if (incomeResult.IsFailure)
                return Result.Failure<List<Income>>(incomeResult.Error);

            incomes.Add(incomeResult.Value);
        }

        return Result.Success(incomes);
    }

    private async Task<Result<List<Expense>>> CreateExpenses(IEnumerable<ExpenseDto> expensesDtos, CancellationToken cancellationToken)
    {
        var incomes = new List<Expense>();
        foreach (var expenseDto in expensesDtos)
        {
            var categoryResult = await _categoryProvider.GetOrCreateAsync(expenseDto.Category, cancellationToken);
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