namespace Domain.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User with this login was not found.")
    {
    }
}