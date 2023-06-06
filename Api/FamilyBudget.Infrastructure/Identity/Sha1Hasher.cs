using System.Security.Cryptography;
using System.Text;

namespace FamilyBudget.Infrastructure.Identity;

public class Sha1Hasher : IStringHasher
{
    public string GenerateHash(string stringToHash)
    {
        using (var sha1 = SHA1.Create())
        {
            var hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(stringToHash));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}