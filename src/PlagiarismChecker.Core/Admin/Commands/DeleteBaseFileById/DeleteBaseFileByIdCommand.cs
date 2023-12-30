using Mediator;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteTrustedFileById;

public sealed record DeleteBaseFileByIdCommand(Guid BaseFileId) : ICommand;