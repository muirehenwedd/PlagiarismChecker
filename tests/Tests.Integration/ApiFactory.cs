using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using PlagiarismChecker.Infrastructure.Data;
using PlagiarismCheck.Api;
using PlagiarismChecker.Infrastructure.Storage;
using Testcontainers.Azurite;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace Tests.Integration;

public sealed class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithHostname("localhost")
            .WithDatabase("plagiarism_checker_test")
            .WithUsername("postgres")
            .WithPassword("12345")
            .Build();

    private readonly AzuriteContainer _blobStorageContainer =
        new AzuriteBuilder()
            .WithHostname("localhost")
            .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IBlobStorageConnectionStringBuilder));

            var substitute = Substitute.For<IBlobStorageConnectionStringBuilder>();
            substitute.GetConnectionString().Returns(_blobStorageContainer.GetConnectionString());

            services.AddSingleton<IBlobStorageConnectionStringBuilder>(_ => substitute);
        });

        builder.ConfigureAppConfiguration((_, conf) =>
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Auth:ApiKey", "ApiKey"},

                {"Auth:Jwt:Issuer", "test"},
                {"Auth:Jwt:Audience", "test"},
                {"Auth:Jwt:TtlSeconds", "3600"},
                {"Auth:Jwt:Secret", new string('a', 256)},

                {"ConnectionStrings:Postgres", $"{_dbContainer.GetConnectionString()};Include Error Detail=true;"},

                {"BlobStorage", "files"},
                {"AllowedMediaTypesOptions", "text/plain"},

                {"PlagiarismCheck:IgnoreNumbers", "false"},
                {"PlagiarismCheck:IgnoreCase", "false"},
                {"PlagiarismCheck:MismatchTolerance", "2"},
                {"PlagiarismCheck:MismatchPercentage", "80"},
                {"PlagiarismCheck:PhraseLength", "6"},
                {"PlagiarismCheck:WordThreshold", "6"}
            };

            conf.AddInMemoryCollection(inMemorySettings);
        });

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated();

        return host;
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _blobStorageContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _blobStorageContainer.StopAsync();
    }
}