using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;
using Microsoft.AspNetCore.Identity;


namespace FamilyBudget.Domain.Entities;

public class User : IdentityUser<Guid>, IAuditableEntity, ISoftDelete
{
    public string PasswordSalt { get; internal set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Budget> Budgets { get; internal set; }

    internal User()
    {
    }

    public void Delete() => IsDeleted = true;

    public static Result<User> Create(string username, string passwordSalt) =>
        Result.Success()
            .Ensure(() => string.IsNullOrEmpty(username) == false, "Username cannot be empty.")
            .Ensure(() => !string.IsNullOrWhiteSpace(passwordSalt), "PasswordSalt cannot be empty.")
            .Map(() => new User
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username,
                PasswordSalt = passwordSalt
            });

    public Result AddBudget(Budget? budget) =>
        Result.Success()
            .Ensure(() => budget != null, "Budget cannot be null.")
            .Tap(() => Budgets.Add(budget!));
}