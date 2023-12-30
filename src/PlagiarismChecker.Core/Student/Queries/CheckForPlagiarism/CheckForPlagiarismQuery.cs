using System.Security.Claims;
using Mediator;

namespace PlagiarismChecker.Core.Student.Queries.CheckForPlagiarism;

public sealed record CheckForPlagiarismQuery(ClaimsPrincipal User, Guid AssignmentId)
    : IQuery<CheckForPlagiarismQueryResult>;