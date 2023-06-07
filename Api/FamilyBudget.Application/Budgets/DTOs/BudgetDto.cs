using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Application.Incomes.DTOs;
using FamilyBudget.Domain.Entities;

namespace FamilyBudget.Application.Budgets.DTOs;

public class BudgetDto
{
    public Guid? BudgetId { get; set; }
    public string Name { get; set; }
    public IEnumerable<IncomeDto> Incomes { get; set; }
    public IEnumerable<ExpenseDto> Expenses { get; set; }
    public IEnumerable<Guid> SharedToUsersIds { get; set; }
}