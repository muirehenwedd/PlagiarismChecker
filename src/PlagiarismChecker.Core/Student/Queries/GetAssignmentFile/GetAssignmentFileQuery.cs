using System.Security.Claims;
using Mediator;

namespace PlagiarismChecker.Core.Student.Queries.GetAssignmentFile;

public sealed record GetAssignmentFileQuery(ClaimsPrincipal User, Guid AssignmentId, Guid AssignmentFileId)
    : IQuery<GetAssignmentFileQueryResult>;