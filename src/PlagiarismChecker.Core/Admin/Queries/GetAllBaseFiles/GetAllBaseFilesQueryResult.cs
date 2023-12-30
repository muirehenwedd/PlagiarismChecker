using PlagiarismChecker.Core.Admin.DTOs;

namespace PlagiarismChecker.Core.Admin.Queries.GetAllTrustedFiles;

public sealed record GetAllBaseFilesQueryResult(IEnumerable<BaseFileDto> Files);