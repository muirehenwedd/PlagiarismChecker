using PlagiarismChecker.Core.Admin.DTOs;

namespace PlagiarismChecker.Core.Admin.Queries.GetAllBaseFiles;

public sealed record GetAllBaseFilesQueryResult(IEnumerable<BaseFileDto> Files);