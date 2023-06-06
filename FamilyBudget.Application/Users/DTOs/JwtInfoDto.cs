namespace FamilyBudget.Application.Users.DTOs;

public class JwtInfoDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}