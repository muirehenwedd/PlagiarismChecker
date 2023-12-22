namespace Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}