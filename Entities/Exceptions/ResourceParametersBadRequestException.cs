namespace Entities.Exceptions;

public sealed class ResourceParametersBadRequestException : BadRequestException
{
    public ResourceParametersBadRequestException() : base("Resource parameters sent from a client is null.")
    {
    }

    public ResourceParametersBadRequestException(string message) : base(message)
    {
    }

    public ResourceParametersBadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }
}