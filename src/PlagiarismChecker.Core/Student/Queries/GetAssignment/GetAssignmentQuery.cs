using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Student.Queries.GetAssignment;

public record GetAssignmentQuery(ClaimsPrincipal User, AssignmentId AssignmentId)
    : IQuery<AssignmentDto>;