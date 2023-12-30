using System.Security.Claims;
using Mediator;

namespace PlagiarismChecker.Core.Student.Commands.DeleteAssignmentFile;

public record DeleteAssignmentFileCommand(ClaimsPrincipal User, Guid AssignmentId, Guid AssignmentFileId) : ICommand;