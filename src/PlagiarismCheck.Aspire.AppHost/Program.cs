var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", true);

var postgres = builder
    .AddPostgres("postgres", password: postgresPassword)
    .WithPgAdmin()
    .WithDataVolume("plagiarism_checker_postgres_volume");

var plagiarismCheckerDatabase = postgres.AddDatabase("postgresdb", "plagiarism_checker");

var blobStorage = builder
    .AddContainer("blobstorage", "mcr.microsoft.com/azure-storage/azurite")
    .WithVolume("plagiarism_checker_blobstorage_volume", "/data")
    .WithArgs("azurite-blob", "--blobHost", "0.0.0.0", "-l", "/data")
    .WithEndpoint(targetPort: 10000, port: 10000, name: "blobstorage-http", scheme: "http");

var api = builder.AddProject<Projects.PlagiarismCheck_Api>("plagiarism-check-api")
    .WithReference(plagiarismCheckerDatabase, "Postgres")
    .WithEnvironment("ConnectionStrings__BlobStorage",
        () => $"UseDevelopmentStorage=true;DevelopmentStorageProxyUri={blobStorage.GetEndpoint("blobstorage-http").Url};");

builder.Build().Run();