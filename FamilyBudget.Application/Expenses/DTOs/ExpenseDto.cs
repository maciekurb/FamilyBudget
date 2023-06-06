namespace FamilyBudget.Application.Expenses.DTOs;

public class ExpenseDto
{
    public Guid? ExpenseId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}