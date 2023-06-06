namespace FamilyBudget.Application.Incomes.DTOs;

public class IncomeDto
{
    public Guid? IncomeId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}