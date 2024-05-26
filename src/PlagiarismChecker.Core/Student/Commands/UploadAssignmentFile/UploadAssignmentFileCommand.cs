using System.Security.Claims;
using Mediator;
using PlagiarismChecker.Core.Student.DTOs;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Student.Commands.UploadAssignmentFile;

public sealed record UploadAssignmentFileCommand(
    ClaimsPrincipal User,
    AssignmentId AssignmentId,
    Stream FileStream,
    string ContentType,
    string FileName
)
    : ICommand<AssignmentFileDto>;