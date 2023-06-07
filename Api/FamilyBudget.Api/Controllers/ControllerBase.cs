using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;
using FamilyBudget.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ControllerBase : Controller
{
    protected Guid UserId
    {
        get
        {
            var userId = User.Claims.First(i => i.Type == ApiClaimType.UserId).Value;

            return Guid.Parse(userId);
        }
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

        if (result.Error == CommonError.Unauthorized || parts.Contains(CommonError.Unauthorized))
            return Unauthorized(result.Error);

        return BadRequest(result.Error);
    }
}