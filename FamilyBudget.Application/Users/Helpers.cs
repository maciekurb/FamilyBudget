using System.IdentityModel.Tokens.Jwt;
using FamilyBudget.Application.Users.DTOs;

namespace FamilyBudget.Application.Users;

public static class JwtSecurityTokenExtensions
{
    public static JwtInfoDto ToDto(this JwtSecurityToken token) =>
        new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        };
}