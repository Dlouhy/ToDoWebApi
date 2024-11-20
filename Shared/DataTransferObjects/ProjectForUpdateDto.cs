namespace Shared.DataTransferObjects;

public class ProjectForUpdateDto : ProjectForManipulationDto
{
    public ICollection<ProjectTaskForUpdateDto>? ProjectTasks { get; init; }
}