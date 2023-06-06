using CSharpFunctionalExtensions;
using FamilyBudget.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ControllerBase : Controller
{
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