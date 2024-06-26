﻿using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Core.Services;

namespace PlagiarismChecker.Infrastructure.Services.Configuration;

public static class ServicesRegistration
{
    internal static IServiceCollection RegisterServices(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddSingleton<IDocumentInitializationService, DocumentInitializationService>()
            .AddSingleton<IFileReaderService, FileReaderService>()
            .AddSingleton<ITokenHasherService, TokenHasherService>()
            .AddSingleton<IHashSorterService, HashSorterService>()
            .AddSingleton<ITokenizerService, TokenizerService>();
}