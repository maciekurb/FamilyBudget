using FamilyBudget.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FamilyBudget.Domain.Tests.Entities;

public class IncomeTests
{
    [Fact]
    public void CreateValidIncome()
    {
        // Arrange
        var amount = 1000m;
        var description = "Salary";
        var category = new Category
        {
            Name = "Salary"
        };

        // Act
        var income = Income.Create(amount, description, category);

        // Assert
        income.IsSuccess.Should()
            .BeTrue();
        
        income.Should()
            .NotBeNull();

        income.Value.Amount.Should()
            .Be(amount);

        income.Value.Description.Should()
            .Be(description);

        income.Value.Category.Should()
            .NotBeNull();

        income.Value.Category.Name.Should()
            .Be(category.Name);
    }

    [Theory]
    [InlineData(0, "Salary", "Amount must be greater than zero.")]
    [InlineData(1000, "", "Description cannot be empty.")]
    [InlineData(1000, null, "Description cannot be empty.")]
    [InlineData(1000, "Salary", "Expense must be linked to some category.")]

    public void CreateIncome_ThrowsDomainUserException(decimal amount, string description, string expectedException)
    {
        // Act
        var result = Income.Create(amount, description, null);

        // Assert
        result.Error
            .Should()
            .Be(expectedException);
    }
}