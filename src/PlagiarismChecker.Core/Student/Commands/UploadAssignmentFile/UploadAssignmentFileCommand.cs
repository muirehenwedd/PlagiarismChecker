using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Core.Student.DTOs;

namespace PlagiarismChecker.Core.Student.Commands.UploadAssignmentFile;

public sealed record UploadAssignmentFileCommand(
    ClaimsPrincipal User,
    Guid AssignmentId,
    Stream FileStream,
    string ContentType,
    string FileName
)
    : ICommand<AssignmentFileDto>;