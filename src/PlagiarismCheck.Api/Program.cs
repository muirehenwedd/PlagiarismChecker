using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using PlagiarismChecker.Core.CoreConfiguration;
using PlagiarismChecker.Domain.Entities;
using PlagiarismChecker.Infrastructure.Data;
using PlagiarismChecker.Infrastructure.InfrastructureConfiguration;
using PlagiarismCheck.Api.Authentication.Configuration;
using PlagiarismCheck.Api.Authorization.Configuration;
using PlagiarismCheck.Api.Endpoints.Configuration;
using PlagiarismCheck.Api.Extensions;
using PlagiarismCheck.Api.Services.Configuration;
using PlagiarismCheck.Aspire.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration;

builder.Services.RegisterPresentationServices();
builder.Services.RegisterCoreServices();
builder.Services.RegisterInfrastructureServices(configuration);
builder.Services.RegisterAuthentication();
builder.Services.RegisterAuthorization();

builder.Services.AddHealthChecks()
    .AddNpgSql(configuration.GetConnectionString("Postgres")!);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await Task.Delay(3_000);
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(_ => { });
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapAdminEndpoints();
app.MapStudentEndpoints();
app.MapIdentityApi<User>();

app.MapGet("/get/asdf/asdf", async () =>
{
    var result = TypedResults.File(new MemoryStream("qwaer"u8.ToArray()), MediaTypeNames.Text.Plain);
    return result;
});

await app.RunAsync();