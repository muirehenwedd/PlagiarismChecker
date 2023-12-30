using Microsoft.EntityFrameworkCore;
using PlagiarismChecker.Infrastructure.Data;

namespace PlagiarismCheck.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();

        return app;
    }
}