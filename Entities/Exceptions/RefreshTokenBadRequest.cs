namespace Entities.Exceptions;

public sealed class RefreshTokenBadRequest : BadRequestException
{
    public RefreshTokenBadRequest() : base("Invalid client request. The tokenDTO has some invalid values.")
    {
    }

    public RefreshTokenBadRequest(string message) : base(message)
    {
    }

    public RefreshTokenBadRequest(string message, Exception innerException) : base(message, innerException)
    {
    }
}