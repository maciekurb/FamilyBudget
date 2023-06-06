using System.Net;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Users;
using FamilyBudget.Application.Users.Commands;
using FamilyBudget.Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Controllers;

public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
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

        if (result.IsFailure)
            return Conflict(result.Error);

        return HandleCommandResult(result);
    }
    
    /// <summary>
    ///     Generates a JWT token for user account with provided credentials.
    /// </summary>
    [HttpPost]
    [Route(nameof(Login))]
    [ProducesResponseType(typeof(JwtInfoDto), (int)HttpStatusCode.OK)]
    public Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken) =>
        _mediator.Send(new LoginCommand(dto.Username, dto.Password), cancellationToken)
            .Map(token => token.ToDto())
            .Finally(HandleCommandResult);
}
