using Mediator;
using PlagiarismChecker.Domain.Entities;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteBaseFileById;

public sealed record DeleteBaseFileByIdCommand(BaseFileId BaseFileId) : ICommand;