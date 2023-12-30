using Mediator;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteTrustedFileByName;

public sealed record DeleteBaseFileByNameCommand(string Name) : ICommand;