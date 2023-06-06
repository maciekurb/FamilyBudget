using FamilyBudget.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FamilyBudget.Domain.Tests.Entities;

public class CategoryTests
{
    [Fact]
    public void CreateValidCategory()
    {
        // Arrange
        var name = "Salary";

        // Act
        var category = Category.Create(name);

        // Assert
        category.IsSuccess.Should()
            .BeTrue();

        category.Should()
            .NotBeNull();

        category.Value.Name.Should()
            .Be(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateCategory_InvalidName_ThrowsDomainException(string category)
    {
        var result = Category.Create(category);

        result.IsFailure.Should()
            .BeTrue();

        result.Error.Should()
            .Be("Name cannot be empty.");
    }
}