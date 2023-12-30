namespace PlagiarismChecker.Core.Student.Queries.GetAssignmentFile;

public record GetAssignmentFileQueryResult(Stream FileContent, string FileName, string ContentType);