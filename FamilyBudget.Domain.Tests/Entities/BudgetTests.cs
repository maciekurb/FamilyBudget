using FamilyBudget.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FamilyBudget.Domain.Tests.Entities;

public class BudgetTests
{
    private readonly User _owner;
    private readonly Income _income;
    private readonly Expense _expense;

    
    public BudgetTests()
    {
        _owner = new User
        {
            Username = "TestUser",
            Email = "testuser@example.com"
        };

        _income = new Income
        {
            Amount = 1000m,
            Description = "Salary",
            Category = new Category
            {
                Name = "Salary"
            }
        };

        _expense = new Expense
        {
            Amount = 500m,
            Description = "Housing",
            Category = new Category
            {
                Name = "Housing"
            }
        };
    }
    
    [Fact]
    public void Budget_Can_Be_Created()
    {
        // Arrange

        var name = "Test Budget";

        // Act
        var budget = Budget.Create(name, _owner);

        // Assert
        budget.Should()
            .NotBeNull();

        budget.Name.Should()
            .Be(name);

        budget.Owner
            .Should()
            .NotBeNull();

        budget.Owner.Email.Should()
            .Be(_owner.Email);
    }

    [Fact]
    public void Budget_Can_Add_Income()
    {
        // Arrange
        var budget = Budget.Create("Test Budget", _owner);

        // Act
        budget.AddIncome(_income);

        // Assert
        budget.Incomes.Should()
            .NotBeEmpty();

        budget.Incomes.Any(x => x.Description == _income.Description)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Budget_Can_Add_Expense()
    {
        // Arrange
        var budget = Budget.Create("Test Budget", _owner);

        // Act
        budget.AddExpense(_expense);

        // Assert
        budget.Expenses.Should()
            .NotBeEmpty();

        budget.Expenses.Any(x => x.Description == _expense.Description)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Budget_Can_Share_With_User()
    {
        // Arrange
        var sharedUser = new User
        {
            Email = "shareduser@example.com",
            Username = "SharedUser"
        };

        var budget = Budget.Create("Test Budget", _owner);

        // Act
        budget.ShareBudget(sharedUser);

        // Assert
        budget.SharedUsers
            .Should()
            .NotBeEmpty();

        budget.SharedUsers.Any(x => x.Email == sharedUser.Email)
            .Should()
            .BeTrue();
    }
}
