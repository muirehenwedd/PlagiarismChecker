using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Core.Student.Options;

public sealed class AllowedMediaTypesOptions : List<string>, IBindableOptions
{
    public static string SectionPath => "AllowedMediaTypesOptions";
}