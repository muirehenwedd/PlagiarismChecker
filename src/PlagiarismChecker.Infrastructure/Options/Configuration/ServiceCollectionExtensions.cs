using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Infrastructure.Options.Configuration;

public static class ServiceCollectionExtensions
{
    internal static OptionsBuilder<TOptions> BindOptionsModel<TOptions, TValidator>(
        this IServiceCollection services,
        IConfiguration configuration
    )
        where TOptions : class, IBindableOptions
        where TValidator : class, IValidator<TOptions>
    {
        services.AddSingleton<IValidator<TOptions>, TValidator>();

        var builder = services
            .AddOptions<TOptions>()
            .Bind(configuration.GetRequiredSection(TOptions.SectionPath))
            .ValidateWithFluentValidator()
            .ValidateOnStart();

        return builder;
    }
}