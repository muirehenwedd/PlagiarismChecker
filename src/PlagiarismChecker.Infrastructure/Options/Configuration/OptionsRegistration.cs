using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Core.Options;
using PlagiarismChecker.Core.Options.Validation;
using PlagiarismChecker.Core.Student.Options;
using PlagiarismChecker.Core.Student.Options.Validation;
using PlagiarismChecker.Infrastructure.Options.Validation;
using PlagiarismChecker.Infrastructure.Storage.Options;

namespace PlagiarismChecker.Infrastructure.Options.Configuration;

public static class OptionsRegistration
{
    internal static IServiceCollection RegisterOptions(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.BindOptionsModel<JwtOptions, JwtOptionsValidator>(configuration);
        services.BindOptionsModel<AuthOptions, AuthOptionsValidator>(configuration);
        services.BindOptionsModel<PlagiarismCheckOptions, PlagiarismCheckOptionsValidator>(configuration);
        services.BindOptionsModel<AllowedMediaTypesOptions, AllowedMediaTypesOptionsValidator>(configuration);
        services.BindOptionsModel<ConnectionStringsOptions, ConnectionStringsOptionsValidator>(configuration);

        services.BindOptionsModel<BlobOptions, BlobOptionsValidator>(configuration).Configure(
            (BlobOptions options, IConfiguration conf) => options.ServiceUri = conf["services:blobstorage:http:0"]!);

        return services;
    }
}