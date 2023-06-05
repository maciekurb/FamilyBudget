using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Application.Incomes.DTOs;
using FamilyBudget.Domain.Entities;

namespace FamilyBudget.Application.Budgets.DTOs;

public class BudgetDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<IncomeDto> Incomes { get; set; } = new List<IncomeDto>();
    public IEnumerable<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>();
}