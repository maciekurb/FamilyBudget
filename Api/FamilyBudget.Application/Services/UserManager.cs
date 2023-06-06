using FamilyBudget.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudget.Application.Services;

public class UserManager : IUserManager
{
    private readonly UserManager<User> _userManager;

    public UserManager(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult?> CreateAsync(User user, string password) =>
        _userManager.CreateAsync(user, password);

    public Task<bool> CheckPasswordAsync(User user, string password) =>
        _userManager.CheckPasswordAsync(user, password);

    public Task<User> FindByUsernameAsync(string userName) =>
        _userManager.FindByNameAsync(userName);

    public Task<IdentityResult> UpdateAsync(User user) =>
        _userManager.UpdateAsync(user);

    public Task<IdentityResult> DeleteAsync(User user) =>
        _userManager.DeleteAsync(user);
}