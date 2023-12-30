using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Core.Student.DTOs;

namespace PlagiarismChecker.Core.Student.Queries.GetAllAssignments;

public sealed record GetAllAssignmentsQuery(ClaimsPrincipal User)
    : IQuery<IEnumerable<AssignmentDto>>;