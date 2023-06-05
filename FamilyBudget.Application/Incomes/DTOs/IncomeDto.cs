namespace FamilyBudget.Application.Incomes.DTOs;

public class IncomeDto
{
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public string Category { get; internal set; }
}