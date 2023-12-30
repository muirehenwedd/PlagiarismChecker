using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Core.Student.DTOs;

namespace PlagiarismChecker.Core.Student.Queries.GetAssignment;

public record GetAssignmentQuery(ClaimsPrincipal User, Guid AssignmentId)
    : IQuery<AssignmentDto>;