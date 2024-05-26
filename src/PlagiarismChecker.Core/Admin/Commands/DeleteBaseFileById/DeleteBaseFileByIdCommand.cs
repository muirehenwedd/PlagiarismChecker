using Mediator;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteTrustedFileById;

public sealed record DeleteBaseFileByIdCommand(BaseFileId BaseFileId) : ICommand;