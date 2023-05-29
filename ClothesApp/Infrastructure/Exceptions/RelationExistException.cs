namespace Infrastructure.Exceptions;

public class RelationExistException : Exception
{
    public RelationExistException()
    {
    }

    public RelationExistException(string message) : base(message)
    {
    }

    public RelationExistException(string message, Exception inner) : base(message, inner)
    {
    }
}