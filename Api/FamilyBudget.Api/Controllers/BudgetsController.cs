using System.Net;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Budgets.Commands;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Application.Budgets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Controllers;

[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Creates a budget.
    /// </summary>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> CreateBudget([FromBody] BudgetDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateBudgetCommand(UserId, dto), cancellationToken);

        if (result.IsFailure)
            return Conflict(result.Error);

        return HandleCommandResult(result);
    }

    /// <summary>
    ///     Get a budget list.
    /// </summary>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public Task<IActionResult> GetBudgets(CancellationToken cancellationToken) =>
        _mediator.Send(new GetUserBudgetsQuery(UserId), cancellationToken)
            .Finally(HandleCommandResult);
}