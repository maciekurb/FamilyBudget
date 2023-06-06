using AutoMapper;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Domain.Entities;

namespace FamilyBudget.Application.Budgets;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Budget, BudgetDto>();
        CreateMap<BudgetDto, Budget>();
    }
}