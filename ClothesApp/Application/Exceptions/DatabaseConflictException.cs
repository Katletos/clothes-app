namespace Application.Exceptions;

public class DatabaseConflictException : Exception
{
    public DatabaseConflictException()
    {
    }

    public DatabaseConflictException(string message) : base(message)
    {
    }

    public DatabaseConflictException(string message, Exception inner) : base(message, inner)
    {
    }
}