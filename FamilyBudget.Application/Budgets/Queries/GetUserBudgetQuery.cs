using AutoMapper;
using CSharpFunctionalExtensions;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Domain.Entities;
using FamilyBudget.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Application.Budgets.Queries;

public record GetUserBudgetsQuery(Guid UserId) : IRequest<Result<List<BudgetDto>>>;

public class GetUserBudgetsQueryHandler : IRequestHandler<GetUserBudgetsQuery, Result<List<BudgetDto>>>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetUserBudgetsQueryHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public Task<Result<List<BudgetDto>>>
        Handle(GetUserBudgetsQuery request, CancellationToken cancellationToken) =>
        Result.Success()
            .Map(() => _appDbContext.Users.Where(x => x.Id == request.UserId)
                .SelectMany(u => u.Budgets).ToListAsync(cancellationToken))
            .Map(budgets => _mapper.Map<List<BudgetDto>>(budgets));
}