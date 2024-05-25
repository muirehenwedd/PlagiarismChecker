using System.Security.Claims;
using Mediator;

namespace PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

public sealed record CheckForPlagiarismQuery(
    ClaimsPrincipal User,
    Guid AssignmentId,
    int? MismatchTolerance,
    int? MismatchPercentage,
    int? PhraseLength,
    int? WordThreshold
)
    : IQuery<CheckForPlagiarismQueryResult>;