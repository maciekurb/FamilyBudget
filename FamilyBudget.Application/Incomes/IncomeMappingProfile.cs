using AutoMapper;
using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Application.Incomes.DTOs;
using FamilyBudget.Domain.Entities;

namespace FamilyBudget.Application.Incomes;

public class IncomeMappingProfile : Profile
{
    public IncomeMappingProfile()
    {
        CreateMap<Income, IncomeDto>()
            .ForMember(dest => dest.IncomeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        
        CreateMap<IncomeDto, Income>();
    }
}