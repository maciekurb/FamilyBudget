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

public record ModifyBudgetCommand(BudgetDto Dto) : IRequest<Result<BudgetDto>>;

public class ModifyBudgetCommandHandler : IRequestHandler<ModifyBudgetCommand, Result<BudgetDto>>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly ICategoryProvider _categoryProvider;

    public ModifyBudgetCommandHandler(AppDbContext appDbContext, IMapper mapper, ICategoryProvider categoryProvider)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _categoryProvider = categoryProvider;
    }

    public Task<Result<BudgetDto>> Handle(ModifyBudgetCommand request, CancellationToken cancellationToken) =>
        Result.Success()
            .Map(async () => await _appDbContext.Users.FindAsync(request.Dto.UserId))
            .Ensure(user => user != null, "User not found")
            .Ensure(_ => request.Dto.BudgetId.HasValue, "Invalid BudgetId")
            .Map(async _ => await _appDbContext.Budgets
                .Include(x => x.Expenses)
                .Include(x => x.Incomes)
                .Include(x => x.SharedUsers)
                .FirstOrDefaultAsync(x => x.Id == request.Dto.BudgetId, cancellationToken))
            .Ensure(budget => budget != null, "Budget not found")
            .Check(budget => budget!.Update(request.Dto.Name))
            .CheckIf(_ => request.Dto.Incomes.Any(),
                async budget => await UpdateIncomes(budget!, request.Dto.Incomes, cancellationToken))
            .CheckIf(_ => request.Dto.Expenses.Any(),
                async budget => await UpdateExpenses(budget!, request.Dto.Expenses, cancellationToken))
            .CheckIf(_ => request.Dto.SharedToUsersIds.Any(),
                async budget =>
                {
                    foreach (var sharedUserId in request.Dto.SharedToUsersIds)
                    {
                        if (budget!.SharedUsers.Any(x => x.Id == sharedUserId)) 
                            return Result.Success();
                        
                        var user = await _appDbContext.Users.FindAsync(sharedUserId, cancellationToken);
                        var result = budget.ShareBudget(user);
                        
                        if (result.IsFailure)
                            return Result.Failure(result.Error);
                    }
                    
                    return Result.Success();
                })
            .Map(budget => _mapper.Map<BudgetDto>(budget));

    private async Task<Result> UpdateIncomes(Budget budget, IEnumerable<IncomeDto> incomeDtos, CancellationToken cancellationToken)
    {
        foreach (var incomeDto in incomeDtos)
        {
            var existingIncome = budget!.Incomes.FirstOrDefault(x => x.Id == incomeDto.IncomeId);
            if (existingIncome != null)
            {
                var category = await _categoryProvider.GetOrCreateAsync(incomeDto.Category, cancellationToken);
                if (category.IsFailure)
                    return Result.Failure(category.Error);
                            
                existingIncome.Update(incomeDto.Amount, incomeDto.Description, category.Value);
            }
            else
            {
                var category = await _categoryProvider.GetOrCreateAsync(incomeDto.Category, cancellationToken);
                var incomeResult = Income.Create(incomeDto.Amount, incomeDto.Description, category.Value);
                if (incomeResult.IsFailure)
                    return Result.Failure<List<Income>>(incomeResult.Error);
            }
        }

        return Result.Success();
    }

    private async Task<Result> UpdateExpenses(Budget budget, IEnumerable<ExpenseDto> expensesDtos, CancellationToken cancellationToken)
    {
        foreach (var expenseDto in expensesDtos)
        {
            var existingIncome = budget!.Incomes.FirstOrDefault(x => x.Id == expenseDto.ExpenseId);
            if (existingIncome != null)
            {
                var category = await _categoryProvider.GetOrCreateAsync(expenseDto.Category, cancellationToken);
                if (category.IsFailure)
                    return Result.Failure(category.Error);
                            
                existingIncome.Update(expenseDto.Amount, expenseDto.Description, category.Value);
            }
            else
            {
                var category = await _categoryProvider.GetOrCreateAsync(expenseDto.Category, cancellationToken);
                var expenseResult = Income.Create(expenseDto.Amount, expenseDto.Description, category.Value);
                if (expenseResult.IsFailure)
                    return Result.Failure<List<Income>>(expenseResult.Error);
            }
        }

        return Result.Success();
    }
}