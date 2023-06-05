using FamilyBudget.Domain.Entities;
using FamilyBudget.Domain.Exceptions;
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
        income.Should()
            .NotBeNull();

        income.Amount.Should()
            .Be(amount);

        income.Description.Should()
            .Be(description);

        income.Category.Should()
            .NotBeNull();

        income.Category.Name.Should()
            .Be(category.Name);
    }

    [Theory]
    [InlineData(0, "Salary", "Amount must be greater than zero.")]
    [InlineData(1000, "", "Description cannot be empty.")]
    [InlineData(1000, null, "Description cannot be empty.")]
    [InlineData(1000, "Salary", "Category cannot be null.")]

    public void CreateIncome_ThrowsDomainUserException(decimal amount, string description, string expectedException)
    {
        // Act
        var result = Assert.Throws<DomainException>(() => Income.Create(amount, description, null));

        // Assert
        result.Message
            .Should()
            .Be(expectedException);
    }
}