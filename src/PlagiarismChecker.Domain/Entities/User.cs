using Microsoft.AspNetCore.Identity;

namespace PlagiarismChecker.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
}