using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;

namespace FamilyBudget.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; internal set; }
    public string Email { get; internal set; }
    public virtual ICollection<Budget> Budgets { get; internal set; }

    internal User()
    {
    }

    public static Result<User> Create(string username, string email) =>
        Result.Success()
            .Ensure(() => string.IsNullOrEmpty(username) == false, "Username cannot be empty.")
            .Ensure(() => string.IsNullOrEmpty(email) == false, "Email cannot be empty.")
            .Map(() => new User
            {
                Username = username,
                Email = email
            });

    public Result AddBudget(Budget? budget) =>
        Result.Success()
            .Ensure(() => budget != null, "Budget cannot be null.")
            .Tap(() => Budgets.Add(budget!));
}