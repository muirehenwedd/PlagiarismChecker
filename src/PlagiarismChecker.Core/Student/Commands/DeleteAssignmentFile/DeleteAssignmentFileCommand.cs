using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Student.Commands.DeleteAssignmentFile;

public record DeleteAssignmentFileCommand(
    ClaimsPrincipal User,
    AssignmentId AssignmentId,
    AssignmentFileId AssignmentFileId
)
    : ICommand;