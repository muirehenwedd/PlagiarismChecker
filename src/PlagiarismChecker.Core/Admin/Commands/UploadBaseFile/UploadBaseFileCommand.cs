using Mediator;

namespace PlagiarismChecker.Core.Admin.Commands.UploadTrustedFile;

public sealed record UploadBaseFileCommand(Stream FileStream, string ContentType, string FileName)
    : ICommand<UploadBaseFileCommandResult>;