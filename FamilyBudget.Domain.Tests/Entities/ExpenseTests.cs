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
        expense.Should()
            .NotBeNull();

        expense.Amount.Should()
            .Be(amount);

        expense.Description.Should()
            .Be(description);

        expense.Category.Should()
            .NotBeNull();

        expense.Category.Name.Should()
            .Be(category.Name);
    }

    [Theory]
    [InlineData(0, "Housing", "Amount must be greater than zero.")]
    [InlineData(500, "", "Description cannot be empty.")]
    [InlineData(500, null, "Description cannot be empty.")]
    [InlineData(500, "Housing", "Category cannot be null.")]

    public void CreateExpense_ThrowsDomainUserException(decimal amount, string description, string expectedException)
    {
        // Act
        var result = Assert.Throws<DomainException>(() => Expense.Create(amount, description, null));

        // Assert
        result.Message
            .Should()
            .Be(expectedException);
    }
}