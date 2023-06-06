using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Services;
using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudget.Application.Users.Commands;

public record LoginCommand(string Username, string Password) : IRequest<Result<JwtSecurityToken>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<JwtSecurityToken>>
    {
        private readonly IUserManager _userManager;
        private readonly AuthSettings _authSettings;

        public LoginCommandHandler(IUserManager userManager,
            IOptions<AuthSettings> authSettings)
        {
            _userManager = userManager;
            _authSettings = authSettings.Value;
        }

        public Task<Result<JwtSecurityToken>> Handle(LoginCommand request, CancellationToken cancellationToken) =>
            Result.Success()
               .Map(() => _userManager.FindByUsernameAsync(request.Username))
               .Ensure(user => user != null, CommonError.Unauthorized)
               .Map(async user =>
                {
                    var correctPassword = await _userManager.CheckPasswordAsync(user, request.Password);

                    return (User: user, CorrectPassword: correctPassword);
                })
               .Ensure(data => data.CorrectPassword, "Unauthorized")
               .Map(data => data.User)
               .Map(user =>
                {
                    var authClaims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName),
                        new(ApiClaimType.UserId, user.Id.ToString()),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));

                    return new JwtSecurityToken(
                        _authSettings.ValidIssuer,
                        _authSettings.ValidAudience,
                        expires: DateTime.Now.AddHours(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
                });
    }