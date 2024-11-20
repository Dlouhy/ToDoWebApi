namespace Entities.Exceptions;

public sealed class ProjectNotFoundException : NotFoundException
{
    public ProjectNotFoundException()
    {
    }

    public ProjectNotFoundException(int projectId) : base($"The project with id: {projectId} doesn't exist in the database.")
    {
    }

    public ProjectNotFoundException(string message) : base(message)
    {
    }

    public ProjectNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}