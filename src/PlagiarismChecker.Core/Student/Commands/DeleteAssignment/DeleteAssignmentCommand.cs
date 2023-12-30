using System.Security.Claims;
using Mediator;

namespace PlagiarismChecker.Core.Student.Commands.DeleteAssignment;

public record DeleteAssignmentCommand(ClaimsPrincipal User, Guid AssignmentId) : ICommand;