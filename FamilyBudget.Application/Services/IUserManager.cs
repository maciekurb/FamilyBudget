using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudget.Application.Services;

[Injectable]
public interface IUserManager
{
    public Task<IdentityResult?> CreateAsync(User user, string password);
    public Task<bool> CheckPasswordAsync(User user, string password);
    public Task<User> FindByUsernameAsync(string userName);
    Task<IdentityResult> UpdateAsync(User user);
    public Task<IdentityResult> DeleteAsync(User user);
}