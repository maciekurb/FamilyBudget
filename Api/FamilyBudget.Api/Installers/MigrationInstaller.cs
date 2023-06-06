using FamilyBudget.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Installers;

public static class MigrationInstaller
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        return app;
    }
}