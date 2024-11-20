namespace Entities.Exceptions;

public sealed class NotValidMappingsBadRequestException : BadRequestException
{
    public NotValidMappingsBadRequestException()
    {
    }

    public NotValidMappingsBadRequestException(string orderBy) : base($"Valid mappings for sort parameter {orderBy} doesn't exist.")
    {
    }

    public NotValidMappingsBadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }
}