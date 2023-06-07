using AutoMapper;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Domain.Entities;

namespace FamilyBudget.Application.Budgets;

public class BudgetMappingProfile : Profile
{
    public BudgetMappingProfile()
    {
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.BudgetId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Incomes, opt => opt.MapFrom(src => src.Incomes)) 
            .ForMember(dest => dest.Expenses, opt => opt.MapFrom(src => src.Expenses)) 
            .ForMember(dest => dest.SharedToUsersIds, opt => opt.MapFrom(src => src.SharedUsers.Select(user => user.Id)));
        
        CreateMap<BudgetDto, Budget>();
    }
}