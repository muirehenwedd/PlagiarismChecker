var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", true);
var jwtSecret = builder.AddParameter("jwt-secret", true);
var apiKey = builder.AddParameter("api-key", true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume("plagiarism_checker_postgres_volume");

var plagiarismCheckerDatabase = postgres.AddDatabase("postgresdb", "plagiarism_checker");

var blobStorage = builder
    .AddContainer("blobstorage", "mcr.microsoft.com/azure-storage/azurite")
    .WithVolume("plagiarism_checker_blobstorage_volume", "/data")
    .WithArgs("azurite-blob", "--blobHost", "0.0.0.0", "-l", "/data")
    .WithEndpoint(targetPort: 10000, scheme: "http");

var api = builder.AddProject<Projects.PlagiarismCheck_Api>("plagiarism-check-api")
    .WithReference(plagiarismCheckerDatabase, "Postgres")
    .WithReference(blobStorage.GetEndpoint("http"))
    .WithEnvironment("Auth__Jwt__Secret", jwtSecret)
    .WithEnvironment("Auth__ApiKey", apiKey);

builder.Build().Run();