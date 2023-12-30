using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Core.Student.DTOs;

namespace PlagiarismChecker.Core.Student.Commands.CreateAssignment;

public sealed record CreateAssignmentCommand(string Name, ClaimsPrincipal User)
    : ICommand<AssignmentDto>;