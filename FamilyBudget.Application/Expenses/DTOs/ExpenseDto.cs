namespace FamilyBudget.Application.Expenses.DTOs;

public class ExpenseDto
{
    public Guid? ExpenseId { get; internal set; }
    public decimal Amount { get; internal set; }
    public string Description { get; internal set; }
    public string Category { get; internal set; }
}