using FamilyBudget.Infrastructure.DependencyInjection;

namespace FamilyBudget.Infrastructure.Identity;

[Injectable]
public interface IStringHasher
{
    string GenerateHash(string stringToHash);
}