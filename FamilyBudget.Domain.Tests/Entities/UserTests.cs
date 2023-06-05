using FamilyBudget.Domain.Entities;
using FamilyBudget.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace FamilyBudget.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void CreateUser_ValidUsernameAndEmail_UserIsCreated()
    {
        // Arrange
        var username = "TestUser";
        var email = "testuser@example.com";

        // Act
        var user = User.Create(username, email);

        // Assert
        user.Should()
            .NotBeNull();

        user.Username.Should()
            .Be(username);

        user.Email.Should()
            .Be(email);
    }

    [Theory]
    [InlineData(null, "testuser@example.com")]
    [InlineData("", "testuser@example.com")]
    [InlineData("TestUser", null)]
    [InlineData("TestUser", "")]
    public void CreateUser_InvalidUsernameOrEmail_ThrowsDomainUserException(string username, string email)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() => User.Create(username, email));
    }
}