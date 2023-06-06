using System.Net;
using AutoMapper;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Users.Commands;
using FamilyBudget.Application.Users.DTOs;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;

    public UsersController(IMediator mediator, IMapper mapper, AppDbContext appDbContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _appDbContext = appDbContext;
    }

    /// <summary>
    ///     Creates a user account.
    /// </summary>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Register([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAccountCommand(dto), cancellationToken);

        if (result.IsFailure)
            return Conflict(result.Error);

        return HandleCommandResult(result);
    }
    
    protected IActionResult HandleCommandResult(Result result) =>
        result.IsSuccess
            ? NoContent()
            : HandleFailure(result);
    
    protected IActionResult HandleCommandResult<T>(Result<T> result) =>
        result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);

    private IActionResult HandleFailure(Result result)
    {
        var parts = result.Error.Split(" : ");

        return BadRequest(result.Error);
    }
}
