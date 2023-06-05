using FamilyBudget.Domain.Entities;
using FamilyBudget.Domain.Exceptions;
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
        category.Should()
            .NotBeNull();

        category.Name.Should()
            .Be(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateCategory_InvalidName_ThrowsDomainException(string category)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() => Category.Create(category));
    }
}