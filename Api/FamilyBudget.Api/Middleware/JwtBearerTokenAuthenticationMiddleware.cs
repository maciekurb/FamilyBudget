using System.Net;
using FamilyBudget.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Middleware;

public class JwtBearerTokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtBearerTokenAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        var authenticateResult = await context.AuthenticateAsync("Bearer");

        if (!authenticateResult.Succeeded)
        {
            await _next.Invoke(context);

            return;
        }

        var userId = Guid.Parse(authenticateResult
            .Principal?
            .Claims
            .FirstOrDefault(x => x.Type == "userId")
            ?.Value!);

        var user = await dbContext.Users.FirstOrDefaultAsync(usr => usr.Id == userId);

        if (user != null)
        {
            await _next.Invoke(context);

            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}