﻿using Mediator;

namespace PlagiarismChecker.Core.Admin.Queries.GetAllTrustedFiles;

public sealed record GetAllBaseFilesQuery : IQuery<GetAllBaseFilesQueryResult>
{
    public static readonly GetAllBaseFilesQuery Instance = new();
}