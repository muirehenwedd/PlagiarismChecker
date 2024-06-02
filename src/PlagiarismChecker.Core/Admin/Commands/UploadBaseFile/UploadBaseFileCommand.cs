using Mediator;

namespace PlagiarismChecker.Core.Admin.Commands.UploadBaseFile;

public sealed record UploadBaseFileCommand(Stream FileStream, string ContentType, string FileName)
    : ICommand<UploadBaseFileCommandResult>;