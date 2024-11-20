namespace Shared.DataTransferObjects;

public class ProjectTaskDto
{
    public int Id { get; set; }
    public string ProjectTaskName { get; set; } = string.Empty;
    public string? ProjectTaskDescription { get; set; }
    public DateTime ProjectTaskStart { get; set; }
    public DateTime? ProjectTaskEnd { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
}