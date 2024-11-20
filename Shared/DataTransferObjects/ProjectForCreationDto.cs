namespace Shared.DataTransferObjects;

public class ProjectForCreationDto : ProjectForManipulationDto
{
    public ICollection<ProjectTaskForCreationDto>? ProjectTasks { get; init; }
}