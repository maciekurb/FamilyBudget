using System.Net.Http.Headers;
using System.Net.Http.Json;
using FamilyBudget.Application.Budgets.DTOs;
using FamilyBudget.Application.Expenses.DTOs;
using FamilyBudget.Application.Incomes.DTOs;
using FamilyBudget.Application.Users.DTOs;
using FamilyBudget.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace FamilyBudget.Api.Tests;

public class BudgetIntegrationTests : IClassFixture<IntegrationTestFactory<Program, AppDbContext>>
{
    private readonly IntegrationTestFactory<Program, AppDbContext> _factory;
    
    private HttpClient _client;
    private BudgetDto _budgetToCreate;
    private string _userName;
    private string _password;

    public BudgetIntegrationTests(IntegrationTestFactory<Program, AppDbContext> factory) => _factory = factory;

    private void Setup()
    {
        _client = _factory.CreateClient();
        _budgetToCreate = new BudgetDto
        {
            Name = "Test Name",
            Incomes = new List<IncomeDto>
            {
                new()
                {
                    Amount = 5000,
                    Description = "Salary",
                    Category = "Salary"
                },
                new()
                {
                    Amount = 500,
                    Description = "Gift",
                    Category = "Gift"
                }
            },
            Expenses = new List<ExpenseDto>
            {
                new()
                {
                    Amount = 2000,
                    Description = "Rent",
                    Category = "Rent"
                },
                new()
                {
                    Amount = 1500,
                    Description = "Groceries",
                    Category = "Groceries"
                }
            },
            SharedToUsersIds = new List<Guid>()
        };
        _userName = "TestUser";
        _password = "Qwe123!@#";
    }

    [Fact]
    public async Task CreateValidBudget()
    {
        //Arrange - Create User
        Setup();
        await CreateUser();
        var bearerToken = await Login();
        
        //Act
        await CreateBudget(bearerToken);
        
        //Assert
        await GetCreatedBudget();
    }
    
    private async Task CreateUser()
    {
        var content = JsonContent.Create(new
        {
            username = _userName,
            password = _password
        });
        var response = await _client.PostAsync("/api/auth", content);
        response.EnsureSuccessStatusCode();
    }
    
    private async Task<string> Login()
    {
        var content = JsonContent.Create(new
        {
            username = _userName,
            password = _password
        });
        
        var response = await _client.PostAsync("api/auth/login", content);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var jwt = JsonConvert.DeserializeObject<JwtInfoDto>(responseBody);

        jwt?.Should()
            .NotBeNull();
        
        jwt?.Token.Should()
            .NotBeNull();

        return jwt!.Token;
    }
    
    private async Task CreateBudget(string bearerToken)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        var content = JsonContent.Create(_budgetToCreate);
        var response = await _client.PostAsync("api/budgets", content);
        
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var responseBudget = JsonConvert.DeserializeObject<BudgetDto>(responseBody);
        
        responseBudget!.BudgetId.HasValue.Should()
            .BeTrue();
    }

    private async Task GetCreatedBudget()
    {
        var response = await _client.GetAsync("api/budgets");
        
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseBudgets = JsonConvert.DeserializeObject<List<BudgetDto>>(responseBody);

        responseBudgets.Count.Should()
            .Be(1);
    }

}