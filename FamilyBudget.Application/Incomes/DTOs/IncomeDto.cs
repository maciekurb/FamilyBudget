namespace FamilyBudget.Application.Incomes.DTOs;

public class IncomeDto
{
    public Guid? IncomeId { get; internal set; }
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public string Category { get; internal set; }
}