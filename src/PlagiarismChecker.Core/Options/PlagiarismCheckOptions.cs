using PlagiarismChecker.Core.Options.Abstractions;

namespace PlagiarismChecker.Core.Options;

public sealed class PlagiarismCheckOptions : IBindableOptions
{
    public static string SectionPath => "PlagiarismCheck";

    public bool IgnoreNumbers { get; set; }
    public bool IgnoreCase { get; set; }
    public int MismatchTolerance { get; set; }
    public int MismatchPercentage { get; set; }
    public int PhraseLength { get; set; }
    public int WordThreshold { get; set; }
}