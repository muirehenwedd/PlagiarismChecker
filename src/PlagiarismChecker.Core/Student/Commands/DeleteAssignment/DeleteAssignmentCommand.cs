using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Student.Commands.DeleteAssignment;

public record DeleteAssignmentCommand(ClaimsPrincipal User, AssignmentId AssignmentId) : ICommand;