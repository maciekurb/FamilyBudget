using FamilyBudget.Domain.Entities;
using FamilyBudget.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace FamilyBudget.Domain.Tests.Entities;

public class ExpenseTests
{
    [Fact]
    public void CreateValidExpense()
    {
        // Arrange
        var amount = 500m;
        var description = "Housing";
        var category = new Category
        {
            Name = "Salary"
        };

        // Act
        var expense = Expense.Create(amount, description, category);

        // Assert
        expense.IsSuccess.Should()
            .BeTrue();
        
        expense.Should()
            .NotBeNull();

        expense.Value.Amount.Should()
            .Be(amount);

        expense.Value.Description.Should()
            .Be(description);

        expense.Value.Category.Should()
            .NotBeNull();

        expense.Value.Category.Name.Should()
            .Be(category.Name);
    }

    [Theory]
    [InlineData(0, "Housing", "Amount must be greater than zero.")]
    [InlineData(500, "", "Description cannot be empty.")]
    [InlineData(500, null, "Description cannot be empty.")]
    [InlineData(500, "Housing", "Expense must be linked to some category.")]

    public void CreateExpense_ThrowsDomainUserException(decimal amount, string description, string expectedException)
    {
        // Act
        var result = Expense.Create(amount, description, null);

        // Assert
        result.Error
            .Should()
            .Be(expectedException);
    }
}