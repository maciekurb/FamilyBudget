using FamilyBudget.Domain.Entities;
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
        user.IsFailure
            .Should()
            .BeFalse();
        
        user.Value.Should()
            .NotBeNull();

        user.Value.UserName.Should()
            .Be(username);

        user.Value.Email.Should()
            .Be(email);
    }

    [Theory]
    [InlineData(null, "testuser@example.com")]
    [InlineData("", "testuser@example.com")]
    [InlineData("TestUser", null)]
    [InlineData("TestUser", "")]
    public void CreateUser_InvalidUsernameOrEmail_ThrowsDomainUserException(string username, string email)
    {
        // Act
        var result = User.Create(username, email);

        // Assert
        result.IsFailure
            .Should()
            .BeTrue();
    }
}