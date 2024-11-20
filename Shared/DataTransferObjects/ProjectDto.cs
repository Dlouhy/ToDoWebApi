namespace Shared.DataTransferObjects;

public class ProjectDto
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? ProjectDescription { get; set; }
    public DateTime ProjectStart { get; set; }
    public DateTime? ProjectEnd { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<ProjectTaskDto>? ProjectTasks { get; set; }
}