using Mediator;

namespace PlagiarismChecker.Core.Admin.Commands.DeleteBaseFileByName;

public sealed record DeleteBaseFileByNameCommand(string Name) : ICommand;