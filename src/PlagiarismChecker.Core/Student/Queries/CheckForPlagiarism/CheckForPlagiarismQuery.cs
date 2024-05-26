using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

public sealed record CheckForPlagiarismQuery(
    ClaimsPrincipal User,
    AssignmentId AssignmentId,
    int? MismatchTolerance,
    int? MismatchPercentage,
    int? PhraseLength,
    int? WordThreshold
)
    : IQuery<CheckForPlagiarismQueryResult>;