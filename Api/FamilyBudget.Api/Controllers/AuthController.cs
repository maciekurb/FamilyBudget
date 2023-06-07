using System.Net;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Users;
using FamilyBudget.Application.Users.Commands;
using FamilyBudget.Application.Users.DTOs;
using FamilyBudget.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Controllers;

public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Creates a user account.
    /// </summary>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAccountCommand(dto), cancellationToken);

        if (result.IsFailure && result.Error == CommonError.UsernameTaken)
            return Conflict(result.Error);

        return HandleCommandResult(result);
    }

    /// <summary>
    ///     Generates a JWT token for user account with provided credentials.
    /// </summary>
    [HttpPost]
    [Route(nameof(Login))]
    [ProducesResponseType(typeof(JwtInfoDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginCommand(dto.Username, dto.Password), cancellationToken);
            
        if (result.IsFailure)
            return HandleCommandResult(result);

        var jwt = result.Value.ToDto();

        return HandleCommandResult(Result.Success(jwt));
    }
        
}
