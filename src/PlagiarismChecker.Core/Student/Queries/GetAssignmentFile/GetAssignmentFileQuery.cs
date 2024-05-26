using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Student.Queries.GetAssignmentFile;

public sealed record GetAssignmentFileQuery(
    ClaimsPrincipal User,
    AssignmentId AssignmentId,
    AssignmentFileId AssignmentFileId
)
    : IQuery<GetAssignmentFileQueryResult>;