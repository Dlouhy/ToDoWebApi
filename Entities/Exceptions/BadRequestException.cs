namespace Entities.Exceptions;

public abstract class BadRequestException : Exception
{
    protected BadRequestException()
    {
    }

    protected BadRequestException(string message) : base(message)
    {
    }

    protected BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }
}