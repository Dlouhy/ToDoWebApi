namespace Entities.Exceptions;

public sealed class ProjectTaskNotFoundException : NotFoundException
{
    public ProjectTaskNotFoundException()
    {
    }

    public ProjectTaskNotFoundException(int projectId, int projectTaskId) : base($"The project task with Id: {projectTaskId} for Project with Id: {projectId} doesn't exist in the database.")
    {
    }

    public ProjectTaskNotFoundException(string message) : base(message)
    {
    }

    public ProjectTaskNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}