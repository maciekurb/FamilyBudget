using System.Net;
using FamilyBudget.Application.Budgets.Commands;
using FamilyBudget.Application.Budgets.DTOs;
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
        var result = await _mediator.Send(new CreateBudgetCommand(dto), cancellationToken);

        if (result.IsFailure)
            return Conflict(result.Error);

        return HandleCommandResult(result);
    }
}