using Microsoft.AspNetCore.StaticFiles;
using PlagiarismCheck.Api.ExceptionHandling.ExceptionHandlers;
using PlagiarismCheck.Api.ExceptionHandling.Configuration;

namespace PlagiarismCheck.Api.Services.Configuration;

public static class PresentationConfigurationExtensions
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

        collection.AddHttpContextAccessor();

        collection.AddExceptionHandler<ValidationExceptionHandler>();
        collection.AddExceptionHandler<CommonExceptionHandler>();
        collection.ConfigureExceptionMapping();
        
        collection.AddEndpointsApiExplorer();
        collection.AddSwaggerGen();

        return collection;
    }
}