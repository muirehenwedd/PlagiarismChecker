namespace Domain.Exceptions;

public sealed class PasswordIsIncorrectException : Exception
{
    public PasswordIsIncorrectException() : base("Password is not correct.")
    {
    }
}