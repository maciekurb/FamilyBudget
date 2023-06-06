using CSharpFunctionalExtensions;
using FamilyBudget.Application.Services;
using FamilyBudget.Application.Users.DTOs;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure.Identity;
using MediatR;

namespace FamilyBudget.Application.Users.Commands;

public record CreateAccountCommand(CreateAccountDto? Dto) : IRequest<Result>;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result>
{
    private readonly IUserManager _userManager;
    private readonly IStringHasher _stringHasher;

    public CreateAccountCommandHandler(IUserManager userManager, IStringHasher stringHasher)
    {
        _userManager = userManager;
        _stringHasher = stringHasher;
    }

    public Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken) =>
        Result.Success()
            .Ensure(() => request.Dto != null, nameof(request.Dto))
            .Map(() => _userManager.FindByUsernameAsync(request.Dto!.Username))
            .Bind(_ => CreateApplicationUserAsync(request.Dto!));
    
    private async Task<Result> CreateApplicationUserAsync(CreateAccountDto dto)
    {
        var passwordSalt = GeneratePasswordSalt();
        var userResult = User.Create(dto.Username, passwordSalt);

        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        var identityResult = await _userManager.CreateAsync(userResult.Value, dto.Password);

        if (identityResult?.Succeeded == false)
            return Result.Failure(string.Join(',', identityResult.Errors.Select(err => err.Code)));

        return Result.Success();
    }
    
    private string GeneratePasswordSalt()
    {
        var rnd = new Random();
        var res = DateTime.Now.Ticks * rnd.Next();

        return _stringHasher.GenerateHash(res.ToString())[..10];
    }
}