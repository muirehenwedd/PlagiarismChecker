using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Core.Common.Services;

namespace PlagiarismChecker.Infrastructure.Services.Configuration;

public static class ServicesRegistration
{
    internal static IServiceCollection RegisterServices(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddSingleton<IGuidGeneratorService, GuidGeneratorService>()
            .AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton<IDocumentInitializationService, DocumentInitializationService>()
            .AddSingleton<IDocumentComparerService, DocumentComparerService>()
            .AddSingleton<IFileReaderService, FileReaderService>()
            .AddSingleton<ITokenHasherService, TokenHasherService>()
            .AddSingleton<IHashSorterService, HashSorterService>()
            .AddSingleton<ITokenizerService, TokenizerService>();
}