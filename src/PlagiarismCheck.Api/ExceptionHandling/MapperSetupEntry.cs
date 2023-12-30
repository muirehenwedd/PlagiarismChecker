using System.Net;

namespace PlagiarismCheck.Api.ExceptionHandling;

public record struct MapperSetupEntry(HttpStatusCode StatusCode, int CustomCode);