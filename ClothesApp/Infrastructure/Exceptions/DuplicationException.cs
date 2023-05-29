namespace Infrastructure.Exceptions;

public class DuplicationException : Exception
{
    public DuplicationException()
    {
    }

    public DuplicationException(string message) : base(message)
    {
    }

    public DuplicationException(string message, Exception inner) : base(message, inner)
    {
    }
}