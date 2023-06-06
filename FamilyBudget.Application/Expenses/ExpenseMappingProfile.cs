using AutoMapper;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Domain.Entities;

namespace FamilyBudget.Application.Expenses;

public class ExpenseMappingProfile : Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        
        CreateMap<ExpenseDto, Expense>();
    }
}